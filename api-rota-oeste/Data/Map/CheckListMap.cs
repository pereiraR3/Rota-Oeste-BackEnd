using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Cliente;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api_rota_oeste.Data.Map
{
    public class CheckListMap : IEntityTypeConfiguration<CheckListModel>
    {
        public void Configure(EntityTypeBuilder<CheckListModel> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

            builder.Property(x => x.UsuarioId)
            .IsRequired();

            builder.HasOne(x => x.Usuario);

            builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(120);

            builder.Property(x => x.DataCriacao);
        }
    }
}

