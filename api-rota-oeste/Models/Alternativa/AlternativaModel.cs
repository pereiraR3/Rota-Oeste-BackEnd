using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Models.RespostaTemAlternativa;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Models.Alternativa;

[Table("alternativa")]
[Index(nameof(Id), nameof(QuestaoId), IsUnique = true)]
public class AlternativaModel
{
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "QuestaoId é necessário")]
    [Column("id_questao")]
    public int QuestaoId { get; set; }
    
    [Required(ErrorMessage = "Descrição é necessário")]
    [Column("descricao",TypeName = "TEXT")]
    public string Descricao { get; set; }
    
    [Required(ErrorMessage = "Código é necessário")]
    [Column("codigo")]
    public int Codigo { get; set; }
    
    [JsonIgnore]
    [ForeignKey("QuestaoId")]
    public virtual QuestaoModel Questao { get; set; }

    [JsonIgnore]
    public virtual List<RespostaTemAlternativaModel> RespostaTemAlternativaModels { get; set; } = new List<RespostaTemAlternativaModel>();
    
    public AlternativaModel(){}
    
    public AlternativaModel(
        
        AlternativaRequestDTO alternativaRequestDto,
        QuestaoModel questaoModel,
        int codigo
        
        )
    {
        this.QuestaoId = alternativaRequestDto.QuestaoId;
        this.Descricao = alternativaRequestDto.Descricao;
        this.Codigo = codigo;
        this.Questao = questaoModel;
    }
    
}