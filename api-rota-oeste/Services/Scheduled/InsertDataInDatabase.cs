using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using api_rota_oeste.Data;
using api_rota_oeste.Models.Alternativa;
using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Models.Usuario;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Services.Scheduled
{
    public class InsertDataInDatabase : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public InsertDataInDatabase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApiDbContext>();

                // Verifica se todas as tabelas principais estão vazias
                bool bancoVazio = !await dbContext.Usuarios.AnyAsync() &&
                                  !await dbContext.Clientes.AnyAsync() &&
                                  !await dbContext.CheckLists.AnyAsync() &&
                                  !await dbContext.Questoes.AnyAsync() &&
                                  !await dbContext.Interacoes.AnyAsync();

                if (bancoVazio)
                {
                    // Criando usuário
                    var usuario = new UsuarioModel()
                    {
                        Nome = "Gabriel Mion",
                        Foto = null,
                        Telefone = "6692337652",
                        Senha = "123123"
                    };

                    dbContext.Usuarios.Add(usuario);
                    await dbContext.SaveChangesAsync();

                    // Adicionando clientes associados ao usuário
                    var clientes = new List<ClienteModel>
                    {
                        new ClienteModel { UsuarioId = usuario.Id, Nome = "Anthony", Telefone = "6692337652" },
                        new ClienteModel { UsuarioId = usuario.Id, Nome = "Vinicius", Telefone = "6593267074" },
                        new ClienteModel { UsuarioId = usuario.Id, Nome = "Alan", Telefone = "6593350010" },
                        new ClienteModel { UsuarioId = usuario.Id, Nome = "Andre", Telefone = "6599908010" },
                        new ClienteModel { UsuarioId = usuario.Id, Nome = "Carlos", Telefone = "6592552181" }
                    };

                    dbContext.Clientes.AddRange(clientes);
                    await dbContext.SaveChangesAsync();

                    // Criando checklist associado ao usuário
                    var checklist = new CheckListModel
                    {
                        UsuarioId = usuario.Id,
                        Nome = "Checklist de Exemplo",
                        DataCriacao = DateTime.Now
                    };

                    dbContext.CheckLists.Add(checklist);
                    await dbContext.SaveChangesAsync();

                    // Adicionando questões associadas ao checklist
                    var questao1 = new QuestaoModel
                    {
                        CheckListId = checklist.Id,
                        Titulo = "Questão 1 - Objetiva",
                        Tipo = TipoQuestao.QUESTAO_OBJETIVA
                    };
                    var questao2 = new QuestaoModel
                    {
                        CheckListId = checklist.Id,
                        Titulo = "Questão 2 - Múltipla Escolha",
                        Tipo = TipoQuestao.QUESTAO_MULTIPLA_ESCOLHA
                    };

                    dbContext.Questoes.AddRange(questao1, questao2);
                    await dbContext.SaveChangesAsync();

                    // Adicionando alternativas para cada questão
                    var alternativas = new List<AlternativaModel>
                    {
                        new AlternativaModel { QuestaoId = questao1.Id, Descricao = "Alternativa A", Codigo = 1 },
                        new AlternativaModel { QuestaoId = questao1.Id, Descricao = "Alternativa B", Codigo = 2 },
                        new AlternativaModel { QuestaoId = questao1.Id, Descricao = "Alternativa C", Codigo = 3 },
                        
                        new AlternativaModel { QuestaoId = questao2.Id, Descricao = "Alternativa A", Codigo = 1 },
                        new AlternativaModel { QuestaoId = questao2.Id, Descricao = "Alternativa B", Codigo = 2 },
                        new AlternativaModel { QuestaoId = questao2.Id, Descricao = "Alternativa C", Codigo = 3 }
                    };

                    dbContext.AlternativaModels.AddRange(alternativas);
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
