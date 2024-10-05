using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using api_rota_oeste.Models.Alternativa;
using api_rota_oeste.Models.RespostaAlternativa;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Models.RespostaTemAlternativa;

[Table("resposta_tem_alternativa")]
[Index(nameof(RespostaId), nameof(AlternativaId), IsUnique = true)]
public class RespostaTemAlternativaModel
{
    
    [Required(ErrorMessage = "RespostaId é necessário")]
    public int RespostaId { get; set; }
    
    [Required(ErrorMessage = "Alternativa é necessário")]
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