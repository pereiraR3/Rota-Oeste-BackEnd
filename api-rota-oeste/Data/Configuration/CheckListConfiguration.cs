using api_rota_oeste.Models.CheckList;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api_rota_oeste.Data.Configuration;

public class CheckListConfiguration : IEntityTypeConfiguration<CheckListModel>
{
    public void Configure(EntityTypeBuilder<CheckListModel> builder)
    {
        // Configurando a relação entre CheckList e Questao
        builder.HasMany(q => q.Questoes)
            .WithOne(r => r.CheckList)
            .HasForeignKey(r => r.CheckListId)
            .OnDelete(DeleteBehavior.Cascade); // Permitir exclusão em cascata para Questao

        // Configurando a relação entre CheckList e ClienteRespondeCheckList
        builder.HasMany(q => q.ClienteRespondeCheckLists)
            .WithOne(r => r.CheckList)
            .HasForeignKey(r => r.CheckListId)
            .OnDelete(DeleteBehavior.NoAction); // Evitar exclusão em cascata para evitar múltiplos caminhos
    }
}