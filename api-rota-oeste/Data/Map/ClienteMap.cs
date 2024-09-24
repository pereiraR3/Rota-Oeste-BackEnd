using api_rota_oeste.Models.Cliente;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api_rota_oeste.Data.Map;

/**
 * Mapeamento Objeto Relacional da entidade Cliente
 */
public class ClienteMap : IEntityTypeConfiguration<ClienteModel>
{
    public void Configure(EntityTypeBuilder<ClienteModel> builder)
    {
        
        // Atributo ID que é auto gerado a cada nova instancia da classe ClienteModel
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        // Atributo UsuarioID é obrigatório 
        builder.Property(x => x.UsuarioId)
            .IsRequired();
        
        // * Atributo que vem com o mapeamento n : 1 de Cliente com  Usuario
        builder.HasOne(x => x.Usuario);
        
        // Atributo nome se trata de um atributo obrigatório 
        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(50);
        
        // Atributo telefone se trata de um atributo obrigatório
        builder.Property(x => x.Telefone)
            .IsRequired()
            .HasMaxLength(11);
        
        builder.HasIndex(x => x.Telefone)
            .IsUnique();
        
        // Atributo Foto se trata de um atributo não obrigatório
        builder.Property(x => x.Foto);
        
    }
}