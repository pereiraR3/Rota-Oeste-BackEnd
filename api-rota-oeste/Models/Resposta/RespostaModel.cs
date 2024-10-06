using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using api_rota_oeste.Models.Alternativa;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Models.RespostaTemAlternativa;

namespace api_rota_oeste.Models.RespostaAlternativa;

/// <summary>
/// Representa a classe <see cref="RespostaModel"/> que descreve uma resposta fornecida para uma questão em uma interação.
/// </summary>
/// <remarks>
/// Esta classe é responsável por armazenar as informações de uma resposta, incluindo a questão e a interação associadas, além de possíveis alternativas e uma foto como resposta.
/// </remarks>
/// <example>
/// Aqui está um exemplo de como usar a <see cref="RespostaModel"/>:
/// <code>
/// var resposta = new RespostaModel(requestDto, interacaoModel, questaoModel);
/// resposta.Foto = fotoBytes;
/// </code>
/// </example>
/// <seealso cref="QuestaoModel"/>
/// <seealso cref="InteracaoModel"/>
/// <seealso cref="RespostaTemAlternativaModel"/>
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
        
        [Column( "foto", TypeName = "VARBINARY(MAX)")]
        public byte[]?  Foto { get; set; }
        
        [JsonIgnore]
        [ForeignKey("InteracaoId")]
        public virtual InteracaoModel Interacao { get; set; }
        
        [JsonIgnore]
        [ForeignKey("QuestaoId")]
        public virtual QuestaoModel Questao { get; set; }
        
        [JsonIgnore]
        public virtual List<RespostaTemAlternativaModel> RespostaTemAlternativaModels { get; set; } = new List<RespostaTemAlternativaModel>();
        
        public RespostaModel(){}
        
        public RespostaModel(
                
                RespostaRequestDTO request, 
                InteracaoModel interacao,
                QuestaoModel questao
                
                )
        {
                
                this.QuestaoId = request.QuestaoId;
                this.InteracaoId = request.InteracaoId;
                this.Foto = request.Foto;
                this.Interacao = interacao;
                this.Questao = questao;
                
        }
        
}
