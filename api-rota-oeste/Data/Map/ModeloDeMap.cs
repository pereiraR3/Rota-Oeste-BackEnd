// using api_rota_oeste.Models;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;

// namespace api_rota_oeste.Data.Map;

// /**
//  * Siga o padrão de mapeamento - este arquivo serve somente para dar um norte
//  */
// public class TarefaMap : IEntityTypeConfiguration<TarefaModel>
// {
//     public void Configure(EntityTypeBuilder<TarefaModel> builder)
//     {
        
//         builder.HasKey(x => x.Id);
//         builder.Property(x => x.Id).ValueGeneratedOnAdd();

//         builder.Property(x => x.Nome).IsRequired().HasMaxLength(255);
//         builder.Property(x => x.Descricao).IsRequired().HasMaxLength(1000);
//         builder.Property(x => x.Status).IsRequired();
//         builder.Property(x => x.UsuarioId);

//         builder.HasOne(x => x.Usuario);

//     }
// }