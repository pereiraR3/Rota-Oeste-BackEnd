using api_rota_oeste.Data;
using api_rota_oeste.Repositories;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services;
using api_rota_oeste.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddTransient<IQuestaoRepository, QuestaoRepository>();
builder.Services.AddTransient<IQuestaoService,QuestaoService>();

builder.Services.AddTransient<IInteracaoRepository, InteracaoRepository>();
builder.Services.AddTransient<IInteracaoService, InteracaoService>();


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

// Add -> mapeamento de controllers
app.MapControllers();

app.Run();
