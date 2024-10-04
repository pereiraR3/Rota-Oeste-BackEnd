using api_rota_oeste.Models.Alternativa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api_rota_oeste.Data.Configuration;

public class AlternativaConfiguration : IEntityTypeConfiguration<AlternativaModel>
{
    
    public void Configure(EntityTypeBuilder<AlternativaModel> builder)
    {
        
        // Configurando a relação entre Alternativa e Questao 
        builder.HasOne(q => q.Questao)
            .WithMany(cl => cl.AlternativaModels)
            .HasForeignKey(q => q.QuestaoId)
            .OnDelete(DeleteBehavior.Cascade); // Configura a exclusão em cascata
        
    }
    
}