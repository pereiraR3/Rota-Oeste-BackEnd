using System.Text;
using api_rota_oeste.Data;
using api_rota_oeste.Models.Token;
using api_rota_oeste.Repositories;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services;
using api_rota_oeste.Services.Interfaces;
using api_rota_oeste.Services.Scheduled;
using api_rota_oeste.Services.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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

//INIT OAUTH
// Adicionando suporte para acessar as configurações da classe TokenSettings
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["TokenSettings:Issuer"],
            ValidAudience = builder.Configuration["TokenSettings:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenSettings:SecretKey"]))
            //ClockSkew = TimeSpan.Zero
        };
    });

//END OAUTH

builder.Services.AddAuthorization();

builder.Services.AddMvc();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar os controladores no contêiner de serviços
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Registrar o DbContext com o nome correto
builder.Services.AddEntityFrameworkSqlServer()
    .AddDbContext<ApiDbContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("Database")),
        ServiceLifetime.Scoped
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

builder.Services.AddScoped<IRespostaRepository, RespostaRepository>();
builder.Services.AddScoped<IRespostaService, RespostaService>();

builder.Services.AddScoped<IAlternativaRepository, AlternativaRepository>();
builder.Services.AddScoped<IAlternativaService, AlternativaService>();

builder.Services.AddScoped<IClienteRespondeCheckListRepository, ClienteRespondeCheckListRepository>();

builder.Services.AddScoped<IRespostaTemAlternativaRepository, RespostaTemAlternativaRepository>();

builder.Services.AddScoped<IRepository, Repository>();

builder.Services.AddScoped<IWhatsAppService, WhatsAppService>();

builder.Services.AddScoped<ICheckListProcessService, CheckListProcessService>();

builder.Services.AddScoped<MessageOrchestrationService>();

builder.Services.AddHostedService<InsertDataInDatabase>();

// Ativando o AutoMapper no contexto de aplicação
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

    // Adicionar segurança para Bearer Token no Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira 'Bearer' [espaço] e o seu token JWT na caixa de texto abaixo.\n\nExemplo: 'Bearer 12345abcdef'"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
    context.Database.Migrate(); // Aplicar todas as migrações

    // Executa o script de criação da função somente após as migrações
    var script = @"
        IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_relatorio_geral_checklist]') AND type in (N'IF', N'TF', N'FN'))
        BEGIN
            EXEC('
                CREATE FUNCTION dbo.fn_relatorio_geral_checklist(@id_checklist INT)
                RETURNS TABLE
                AS
                RETURN
                (
                    SELECT 
                        i.id AS id_interacao,
                        c.nome AS nome_cliente,
                        ch.nome AS nome_checklist,
                        i.data_criacao AS data_interacao,
                        q.titulo AS questao,
                        r.id AS id_resposta,
                        rt.id_alternativa AS alternativa
                    FROM 
                        dbo.interacao i
                    JOIN 
                        dbo.cliente c ON i.id_cliente = c.id
                    JOIN 
                        dbo.checklist ch ON i.id_checklist = ch.id
                    JOIN 
                        dbo.resposta r ON r.id_interacao = i.id
                    JOIN 
                        dbo.resposta_tem_alternativa rt ON rt.id_resposta = r.id
                    JOIN 
                        dbo.questao q ON r.id_questao = q.id
                    WHERE 
                        i.status = 1
                        AND i.id_checklist = @id_checklist
                );
            ')
        END;
    ";
    
    context.Database.ExecuteSqlRaw(script); // Executa o script após migrações serem aplicadas
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Adicionar o middleware de tratamento global de exceções
app.UseMiddleware<api_rota_oeste.Middlewares.ExceptionHandlingMiddleware>();

// Configurar para usar o CORS
app.UseCors("PermitirTodos");

// Add -> mapeamento de controllers
app.MapControllers();

app.Run();
