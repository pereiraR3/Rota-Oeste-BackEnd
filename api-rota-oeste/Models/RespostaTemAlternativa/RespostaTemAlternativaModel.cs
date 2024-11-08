using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using api_rota_oeste.Models.Alternativa;
using api_rota_oeste.Models.RespostaAlternativa;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Models.RespostaTemAlternativa;

/// <summary>
/// Representa a classe <see cref="RespostaTemAlternativaModel"/> que descreve a relação entre uma resposta e uma alternativa.
/// </summary>
/// <remarks>
/// Esta classe é responsável por armazenar as informações do relacionamento entre uma resposta e uma alternativa, indicando quais alternativas foram selecionadas para uma determinada resposta.
/// </remarks>
/// <example>
/// Aqui está um exemplo de como usar a <see cref="RespostaTemAlternativaModel"/>:
/// <code>
/// var respostaTemAlternativa = new RespostaTemAlternativaModel(1, 2, respostaModel, alternativaModel);
/// </code>
/// </example>
/// <seealso cref="RespostaModel"/>
/// <seealso cref="AlternativaModel"/>
[Table("resposta_tem_alternativa")]
[Index(nameof(RespostaId), nameof(AlternativaId), IsUnique = true)]
public class RespostaTemAlternativaModel
{
    
    [Required(ErrorMessage = "RespostaId é necessário")]
    [Column("id_resposta")]
    public int RespostaId { get; set; }
    
    [Required(ErrorMessage = "Alternativa é necessário")]
    [Column("id_alternativa")]
    public int AlternativaId { get; set; }

    [JsonIgnore]
    [ForeignKey("AlternativaId")]
    public virtual AlternativaModel Alternativa { get; set; }
    
    [JsonIgnore]
    [ForeignKey("RespostaId")]
    public virtual RespostaModel Resposta { get; set; }
    
    public RespostaTemAlternativaModel() { }
    
    public RespostaTemAlternativaModel(
        
        int respostaId,
        int alternativaId,
        RespostaModel respostaModel,
        AlternativaModel alternativaModel
        
        )
    {
        this.AlternativaId = alternativaId;
        this.RespostaId = respostaId;
        this.Alternativa = alternativaModel;
        this.Resposta = respostaModel;
    }
    
    
}