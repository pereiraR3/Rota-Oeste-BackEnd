using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Models.Questao;

namespace api_rota_oeste.Models.RespostaAlternativa;

[Table("resposta_alternativa")]
public class RespostaAlternativaModel
{
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "QuestaoId é necessário")]
        public int QuestaoId { get; set; }
        
        [Required(ErrorMessage = "InteracaoId é necessário")]
        public int InteracaoId { get; set; }
        
        [JsonIgnore]
        [ForeignKey("InteracaoId")]
        public virtual InteracaoModel Interacao { get; set; }
        
        [JsonIgnore]
        [ForeignKey("QuestaoId")]
        public virtual QuestaoModel? Questao { get; set; }
        
        public int Alternativa { get; set; }
        
        public RespostaAlternativaModel(){}
        
        public RespostaAlternativaModel(
                
                RespostaAlternativaRequestDTO request, 
                InteracaoModel interacao,
                QuestaoModel questao
                
                )
        {
                
                this.QuestaoId = request.QuestaoId;
                this.InteracaoId = request.InteracaoId;
                this.Alternativa = request.Alternativa;
                this.Interacao = interacao;
                this.Questao = questao;
                
        }
        
}
