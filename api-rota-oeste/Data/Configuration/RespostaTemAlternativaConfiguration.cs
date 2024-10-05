using api_rota_oeste.Models.RespostaTemAlternativa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api_rota_oeste.Data.Configuration;

public class RespostaTemAlternativaConfiguration : IEntityTypeConfiguration<RespostaTemAlternativaModel>
{
    public void Configure(EntityTypeBuilder<RespostaTemAlternativaModel> builder)
    {
        
        // Configurando a chave primária composta
        builder.HasKey(cr => new { cr.AlternativaId, cr.RespostaId });
        
        builder
            .HasOne(cr => cr.Alternativa)
            .WithMany(c => c.RespostaTemAlternativaModels)
            .HasForeignKey(cr => cr.AlternativaId)
            .OnDelete(DeleteBehavior.Cascade); 
        
        builder
            .HasOne(cr => cr.Alternativa)
            .WithMany(c => c.RespostaTemAlternativaModels)
            .HasForeignKey(cr => cr.AlternativaId)
            .OnDelete(DeleteBehavior.Cascade); 
        
    }
}