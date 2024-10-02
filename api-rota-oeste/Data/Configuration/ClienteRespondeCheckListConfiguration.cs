using api_rota_oeste.Models.ClienteRespondeCheckList;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api_rota_oeste.Data.Configuration;

public class ClienteRespondeCheckListConfiguration : IEntityTypeConfiguration<ClienteRespondeCheckListModel>
{
    public void Configure(EntityTypeBuilder<ClienteRespondeCheckListModel> builder)
    {
        // Configurando a chave primária composta
        builder.HasKey(cr => new { cr.ClienteId, cr.CheckListId });

        // Definindo as colunas de chaves estrangeiras
        builder
            .HasOne(cr => cr.Cliente)
            .WithMany(c => c.ClienteRespondeCheckLists)
            .HasForeignKey(cr => cr.ClienteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(cr => cr.CheckList)
            .WithMany(c => c.ClienteRespondeCheckLists)
            .HasForeignKey(cr => cr.CheckListId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}