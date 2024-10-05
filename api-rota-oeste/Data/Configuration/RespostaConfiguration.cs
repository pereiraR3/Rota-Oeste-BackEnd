using api_rota_oeste.Models.RespostaAlternativa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api_rota_oeste.Data.Configuration;

public class RespostaConfiguration : IEntityTypeConfiguration<RespostaModel>
{
    public void Configure(EntityTypeBuilder<RespostaModel> builder)
    {
        builder.HasKey(r => r.Id);

        // Relação com RespostaTemAlternativa
        builder.HasMany(q => q.RespostaTemAlternativaModels)
            .WithOne(r => r.Resposta)
            .HasForeignKey(r => r.RespostaId)
            .OnDelete(DeleteBehavior.Restrict); // Evitar exclusão em cascata para evitar múltiplos caminhos
        
        // Relação com Interacao
        builder.HasOne(r => r.Interacao)
            .WithMany(i => i.RespostaAlternativaModels)
            .HasForeignKey(r => r.InteracaoId)
            .OnDelete(DeleteBehavior.Restrict); // Evitar exclusão em cascata para evitar múltiplos caminhos

        // Relação com Questao
        builder.HasOne(r => r.Questao)
            .WithMany(q => q.RespostaModels)
            .HasForeignKey(r => r.QuestaoId)
            .OnDelete(DeleteBehavior.NoAction); // Evitar exclusão em cascata para evitar múltiplos caminhos
    }
}
