using api_rota_oeste.Data.Configuration;
using api_rota_oeste.Models.Alternativa;
using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.ClienteRespondeCheckList;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Models.RespostaAlternativa;
using api_rota_oeste.Models.RespostaTemAlternativa;
using api_rota_oeste.Models.Token;
using api_rota_oeste.Models.Usuario;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace api_rota_oeste.Data;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }
    
    public DbSet<UsuarioModel> Usuarios { get; set; }
    public DbSet<ClienteModel> Clientes { get; set; }
    public DbSet<CheckListModel> CheckLists { get; set; }
    public DbSet<QuestaoModel> Questoes { get; set; }
    public DbSet<InteracaoModel> Interacoes { get; set; }
    public DbSet<RespostaModel> RespostaModels { get; set; }
    public DbSet<ClienteRespondeCheckListModel> ClienteRespondeCheckListModels { get; set; }
    public DbSet<AlternativaModel> AlternativaModels { get; set; }
    
    public DbSet<RespostaTemAlternativaModel> RespostaTemAlternativaModels { get; set; }

    public DbSet<TokenModel> RefreshTokens { get; set; } 
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.ApplyConfiguration(new InteracaoConfiguration());
        modelBuilder.ApplyConfiguration(new ClienteRespondeCheckListConfiguration());
        modelBuilder.ApplyConfiguration(new QuestaoConfiguration());
        modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
        modelBuilder.ApplyConfiguration(new ClienteConfiguration());
        modelBuilder.ApplyConfiguration(new CheckListConfiguration());
        modelBuilder.ApplyConfiguration(new RespostaConfiguration());
        modelBuilder.ApplyConfiguration(new AlternativaConfiguration());
        modelBuilder.ApplyConfiguration(new RespostaTemAlternativaConfiguration());
        
        modelBuilder.Entity<TokenModel>(entity =>
        {
            entity.HasKey(e => e.Id); // Definir a chave primária
            entity.Property(e => e.Token).IsRequired(); // O token é obrigatório
            entity.Property(e => e.Username).IsRequired(); // O nome de usuário é obrigatório
            entity.Property(e => e.Expiration).IsRequired(); // A expiração é obrigatória
            entity.Property(e => e.IsRevoked).IsRequired(); // A revogação é obrigatória
        });
        
        base.OnModelCreating(modelBuilder);       
        
    }
    
}
