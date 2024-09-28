using api_rota_oeste.Data.Map;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Models.Usuario;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Data;

public class ApiDBContext : DbContext
{

    public ApiDBContext(DbContextOptions<ApiDBContext> options) : base  (options)
    {
            
    }
        
    public DbSet<UsuarioModel?> Usuarios { get; set; }
    public DbSet<ClienteModel?> Clientes { get; set; }
    
    public DbSet<QuestaoModel?> Questoes { get; set; }
    public DbSet<InteracaoModel?> Interacoes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
            
        modelBuilder.ApplyConfiguration(new UsuarioMap());
        modelBuilder.ApplyConfiguration(new ClienteMap());

        base.OnModelCreating(modelBuilder);
    }
}
