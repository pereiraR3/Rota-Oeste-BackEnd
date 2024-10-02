using api_rota_oeste.Data.Configuration;
using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.ClienteRespondeCheckList;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Models.RespostaAlternativa;
using api_rota_oeste.Models.Usuario;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Data;

public class ApiDBContext : DbContext
{
    public ApiDBContext(DbContextOptions<ApiDBContext> options) : base(options) { }
        
    public DbSet<UsuarioModel> Usuarios { get; set; }
    public DbSet<ClienteModel> Clientes { get; set; }
    public DbSet<CheckListModel> CheckLists { get; set; }
    public DbSet<QuestaoModel> Questoes { get; set; }
    
    public DbSet<InteracaoModel> Interacoes { get; set; }
    
    public DbSet<RespostaAlternativaModel> RespostaAlternativaModels { get; set; }
    
    public DbSet<ClienteRespondeCheckListModel> ClienteRespondeCheckListModels { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.ApplyConfiguration(new InteracaoConfiguration());
        modelBuilder.ApplyConfiguration(new ClienteRespondeCheckListConfiguration());
        modelBuilder.ApplyConfiguration(new QuestaoConfiguration());
        modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
        modelBuilder.ApplyConfiguration(new ClienteConfiguration());
        modelBuilder.ApplyConfiguration(new CheckListConfiguration());
        
        base.OnModelCreating(modelBuilder);       
        
    }
    
}
