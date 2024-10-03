using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using api_rota_oeste.Models.Questao;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Models.Alternativa;

[Table("alternativa")]
[Index(nameof(Id), nameof(QuestaoId), IsUnique = true)]
public class AlternativaModel
{
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "QuestaoId é necessário")]
    [Column("id_questao")]
    public int QuestaoId { get; set; }
    
    [Required(ErrorMessage = "Descrição é necessário")]
    [Column("descricao",TypeName = "TEXT")]
    public string Descricao { get; set; }
    
    [Required(ErrorMessage = "Código é necessário")]
    public int Codigo { get; set; }
    
    [JsonIgnore]
    [ForeignKey("QuestaoId")]
    public virtual QuestaoModel Questao { get; set; }

    public AlternativaModel(
        
        AlternativaRequestDTO alternativaRequestDTO,
        QuestaoModel questaoModel,
        int codigo
        
        )
    {
        this.QuestaoId = alternativaRequestDTO.QuestaoId;
        this.Descricao = alternativaRequestDTO.Descricao;
        this.Codigo = codigo;
        this.Questao = questaoModel;
    }
    
}