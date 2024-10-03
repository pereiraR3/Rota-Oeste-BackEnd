using api_rota_oeste.Models.Cliente;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api_rota_oeste.Data.Configuration;

public class ClienteConfiguration : IEntityTypeConfiguration<ClienteModel>
{
    public void Configure(EntityTypeBuilder<ClienteModel> builder)
    {
        // Configurando a relação entre Cliente e ClienteRespondeCheckList
        builder.HasMany(q => q.ClienteRespondeCheckLists)
            .WithOne(r => r.Cliente)
            .HasForeignKey(r => r.ClienteId)
            .OnDelete(DeleteBehavior.NoAction); // Evitar exclusão em cascata para evitar múltiplos caminhos

        // Configurando a relação entre Cliente e Interacao
        builder.HasMany(q => q.Interacoes)
            .WithOne(r => r.Cliente)
            .HasForeignKey(r => r.ClienteId)
            .OnDelete(DeleteBehavior.Cascade); // Permitir exclusão em cascata para Interacoes
    }
}