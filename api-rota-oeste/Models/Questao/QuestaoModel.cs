using System.Text.Json.Serialization;
using api_rota_oeste.Models.CheckList;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api_rota_oeste.Models.Alternativa;
using api_rota_oeste.Models.RespostaAlternativa;

namespace api_rota_oeste.Models.Questao;

/// <summary>
/// Representa a classe <see cref="QuestaoModel"/> que descreve uma questão associada a um checklist.
/// </summary>
/// <remarks>
/// Esta classe é responsável por armazenar as informações de uma questão, como título, tipo, e a relação com o checklist, respostas e alternativas associadas.
/// </remarks>
/// <example>
/// Aqui está um exemplo de como usar a <see cref="QuestaoModel"/>:
/// <code>
/// var questao = new QuestaoModel(requestDto, checkList);
/// questao.Titulo = "Título da Questão";
/// questao.Tipo = TipoQuestao.MultiplaEscolha;
/// </code>
/// </example>
/// <seealso cref="CheckListModel"/>
/// <seealso cref="AlternativaModel"/>
/// <seealso cref="RespostaModel"/>
[Table("questao")]
public class QuestaoModel{
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "CheckListId é necessário")]
    [Column("id_checklist")]
    public int CheckListId { get; set; }
    
    [Required(ErrorMessage = "Titulo é necessário")]
    [StringLength(120, ErrorMessage = "O titulo não pode exceder 120 caracteres")]
    [Column("titulo")]
    public string Titulo { get; set; }
    
    [Required(ErrorMessage = "Tipo é necessário")]
    [EnumDataType(typeof(TipoQuestao))]
    [Column("tipo")]
    public TipoQuestao Tipo { get; set; }
    
    [JsonIgnore]
    [ForeignKey("CheckListId")]
    public virtual CheckListModel? CheckList { get; set; }
    
    [JsonIgnore]
    public virtual List<RespostaModel> RespostaModels { get; set; } = new List<RespostaModel>();

    [JsonIgnore]
    public virtual List<AlternativaModel> AlternativaModels { get; set; } = new List<AlternativaModel>();
    
    public QuestaoModel(){}
    
    public QuestaoModel(QuestaoRequestDTO requestDto, CheckListModel checkList)
    {
        
        if(requestDto.CheckListId != 0)
            this.CheckListId = requestDto.CheckListId;
        else
            this.CheckListId = checkList.Id;
        
        this.Titulo = requestDto.Titulo;
        
        this.Tipo = requestDto.Tipo;
        
        this.CheckList = checkList;
        
    }
    
}
