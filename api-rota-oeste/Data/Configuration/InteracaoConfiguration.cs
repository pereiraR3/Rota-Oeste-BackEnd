using api_rota_oeste.Models.Interacao;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api_rota_oeste.Data.Configuration;

public class InteracaoConfiguration : IEntityTypeConfiguration<InteracaoModel>
{
    public void Configure(EntityTypeBuilder<InteracaoModel> builder)
    {
        
        builder
            .HasOne(i => i.Cliente)
            .WithMany()
            .HasForeignKey(i => i.ClienteId)
            .OnDelete(DeleteBehavior.Restrict); // Use Restrict para impedir a exclusão em cascata
        
            builder
            .HasOne(i => i.CheckList)
            .WithMany()
            .HasForeignKey(i => i.CheckListId)
            .OnDelete(DeleteBehavior.Restrict); // Use Restrict para impedir a exclusão em cascata
            
    }
}