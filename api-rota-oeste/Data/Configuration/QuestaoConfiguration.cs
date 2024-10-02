using api_rota_oeste.Models.Questao;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api_rota_oeste.Data.Configuration;

public class QuestaoConfiguration : IEntityTypeConfiguration<QuestaoModel>
{
    public void Configure(EntityTypeBuilder<QuestaoModel> builder)
    {
        
        // Configurando a relação entre Questao e RespostaAlternativa
        builder.HasMany(q => q.RespostaAlternativaModels)
            .WithOne(r => r.Questao)
            .HasForeignKey(r => r.QuestaoId)
            .OnDelete(DeleteBehavior.Cascade); // Configura a exclusão em cascata

        // Configurando a relação entre CheckList e Questao
        builder.HasOne(q => q.CheckList)
            .WithMany(cl => cl.Questoes)
            .HasForeignKey(q => q.CheckListId)
            .OnDelete(DeleteBehavior.Cascade); // Configura a exclusão em cascata
    }
}