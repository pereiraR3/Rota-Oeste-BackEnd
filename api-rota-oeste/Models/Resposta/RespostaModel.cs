using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Models.Questao;

namespace api_rota_oeste.Models.RespostaAlternativa;

[Table("resposta")]
public class RespostaModel
{
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "QuestaoId é necessário")]
        [Column("id_questao")]
        public int QuestaoId { get; set; }
        
        [Required(ErrorMessage = "InteracaoId é necessário")]
        [Column("id_interacao")]
        public int InteracaoId { get; set; }
        
        [Column("alternativa")]
        public int? Alternativa { get; set; }
        
        [Column( "foto", TypeName = "VARBINARY(MAX)")]
        public byte[]?  Foto { get; set; }
        
        [JsonIgnore]
        [ForeignKey("InteracaoId")]
        public virtual InteracaoModel Interacao { get; set; }
        
        [JsonIgnore]
        [ForeignKey("QuestaoId")]
        public virtual QuestaoModel Questao { get; set; }
        
        public RespostaModel(){}
        
        public RespostaModel(
                
                RespostaRequestDTO request, 
                InteracaoModel interacao,
                QuestaoModel questao
                
                )
        {
                
                this.QuestaoId = request.QuestaoId;
                this.InteracaoId = request.InteracaoId;
                this.Alternativa = request.Alternativa;
                this.Foto = request.Foto;
                this.Interacao = interacao;
                this.Questao = questao;
                
        }
        
}
