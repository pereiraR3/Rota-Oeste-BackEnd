using api_rota_oeste.Models.Usuario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api_rota_oeste.Data.Configuration;

public class UsuarioConfiguration : IEntityTypeConfiguration<UsuarioModel>
{
    public void Configure(EntityTypeBuilder<UsuarioModel> builder)
    {
        
        // Configurando a relação entre Usuario e Cliente
        builder.HasMany(q => q.Clientes)
            .WithOne(r => r.Usuario)
            .HasForeignKey(r => r.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade); // Configura a exclusão em cascata
        
        // Configurando a relação entre Usuario e CheckList
        builder.HasMany(q => q.CheckLists)
            .WithOne(r => r.Usuario)
            .HasForeignKey(r => r.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade); // Configura a exclusão em cascata
        
    }
}