// using api_rota_oeste.Data.Map;
// using api_rota_oeste.Models;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Data

{
    public class ApiDBContext : DbContext
    {

        public ApiDBContext(DbContextOptions<ApiDBContext> options) : base  (options)
        {
            
        }
        
        // public DbSet<UsuarioModel> Usuarios { get; set; }
        // public DbSet<TarefaModel> Tarefas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            // modelBuilder.ApplyConfiguration(new UsuarioMap());
            // modelBuilder.ApplyConfiguration(new TarefaMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}