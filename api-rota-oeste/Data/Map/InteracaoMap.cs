
using api_rota_oeste.Models.Interacao;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api_rota_oeste.Data.Map
{
    public class InteracaoMap : IEntityTypeConfiguration<InteracaoModel>
    {
        public void Configure(EntityTypeBuilder<InteracaoModel> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

            builder.Property(x => x.ClienteId)
            .IsRequired();

            builder.HasOne(x => x.Cliente);

            builder.Property(x => x.CheckListId).IsRequired();

            builder.HasOne(x => x.CheckList);
            
            builder.Property(x => x.Data);

            builder.Property(x => x.Status);
        }
    }
}
