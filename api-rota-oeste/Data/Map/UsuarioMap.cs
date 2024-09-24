using api_rota_oeste.Models.Usuario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api_rota_oeste.Data.Map;

/**
 * Mapemaento Objeto Relacional da entidade Usuario
 */
public class UsuarioMap : IEntityTypeConfiguration<UsuarioModel>
{
    
    public void Configure(EntityTypeBuilder<UsuarioModel> builder)
    {
        
        // Configurando o ID auto gerado da classe Usuario
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();
        
        // Atributo Telefone se trata de uma informação obrigatória
        builder.Property(x => x.Telefone)
            .IsRequired()
            .HasMaxLength(11);
        
        builder.HasIndex(x => x.Telefone)
            .IsUnique();
        
        // Atributo Nome se trata de uma informação obrigatória
        builder.Property(x=> x.Nome)
            .IsRequired()
            .HasMaxLength(50);
        
        // Atributo Senha se trata de uma informação obrigatória
        builder.Property(x => x.Senha)
            .IsRequired()
            .HasMaxLength(60);
        
        // Atributo foto (não estruturado) se trata de uma informação não obrigatória
        builder.Property(x => x.Foto);

        // Mapeamento de todos os clientes relacionados a determinado usuario
        builder.HasMany(x => x.Clientes)
            .WithOne(x => x.Usuario)
            .HasForeignKey(x => x.UsuarioId);

    }
    
}