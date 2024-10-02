using api_rota_oeste.Data;
using api_rota_oeste.Repositories;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services;
using api_rota_oeste.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodos",
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar os controladores no contêiner de serviços
builder.Services.AddControllers();

// Registrar o DbContext com o nome correto
builder.Services.AddEntityFrameworkSqlServer()
    .AddDbContext<ApiDBContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("Database"))
    );

// Permitir a injecao dos serviços abaixo, no contexto de aplicação

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();

builder.Services.AddScoped<ICheckListRepository, CheckListRepository>();
builder.Services.AddScoped<ICheckListService, CheckListService>();

builder.Services.AddScoped<IQuestaoRepository, QuestaoRepository>();
builder.Services.AddScoped<IQuestaoService,QuestaoService>();

builder.Services.AddScoped<IInteracaoRepository, InteracaoRepository>();
builder.Services.AddScoped<IInteracaoService, InteracaoService>();

builder.Services.AddScoped<IRespostaAlternativaRepository, RespostaAlternativaRepository>();
builder.Services.AddScoped<IRespostaAlternativaService, RespostaAlternativaService>();

builder.Services.AddScoped<IClienteRespondeCheckListRepository, ClienteRespondeCheckListRepository>();

builder.Services.AddScoped<IRepository, Repository>();

// Registrar o serviço WhatsAppService com HttpClient
builder.Services.AddHttpClient<IWhatsAppService, WhatsAppService>(client =>
{
    client.BaseAddress = new Uri("https://graph.facebook.com/v20.0/399397036595516/messages");
    client.DefaultRequestHeaders.Add("Authorization", $"Bearer EAAWgnQyeEUEBO3J2Vw5trFeqpFsYHxSHAaSN4Pg3cHLZBxZBY2ZAqoPP5qzTIZC9XTMGlPuWmbnOC62ZCLNZCVNHrDmpypMMoSsRpwjy2mmGGrDrCKR84l9wbYHAOyTPp0ktAK0bWTcXXp2APyggC1Q6PbNi6o0BZCWQyHEZAQpgpoHXlTsCR59WDZCK9fO1Qx9qcovaZBtgVMhqhtX8IjxPMSsNY0kEQZD");
});

// Ativando o AutoMapper no contexto de aplicação
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Habilitando as anotações do Swagger
builder.Services.AddSwaggerGen(c =>
    c.EnableAnnotations()
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Adicionar o middleware de tratamento global de exceções
app.UseMiddleware<api_rota_oeste.Middlewares.ExceptionHandlingMiddleware>();

// Configurar para usar o CORS
app.UseCors("PermitirTodos");

// Add -> mapeamento de controllers
app.MapControllers();

app.Run();
