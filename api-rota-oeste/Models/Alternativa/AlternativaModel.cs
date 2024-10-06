using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Models.RespostaTemAlternativa;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Models.Alternativa;

/// <summary>
/// Representa a classe <see cref="AlternativaModel"/> que descreve a alternativa de uma questão.
/// </summary>
/// <remarks>
/// Esta classe é responsável por armazenar as informações de uma alternativa, como sua descrição, código, e a relação com a questão associada.
/// </remarks>
/// <example>
/// Aqui está um exemplo de como usar a <see cref="AlternativaModel"/>:
/// <code>
/// var alternativa = new AlternativaModel(alternativaRequestDto, questaoModel, 1);
/// alternativa.Descricao = "Descrição da alternativa";
/// </code>
/// </example>
/// <seealso cref="QuestaoModel"/>
/// <seealso cref="RespostaTemAlternativaModel"/>
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