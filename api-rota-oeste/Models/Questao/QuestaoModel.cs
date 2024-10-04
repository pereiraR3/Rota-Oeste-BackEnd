using System.Text.Json.Serialization;
using api_rota_oeste.Models.CheckList;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api_rota_oeste.Models.Alternativa;
using api_rota_oeste.Models.RespostaAlternativa;

namespace api_rota_oeste.Models.Questao;

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
    public virtual List<RespostaModel> RespostaAlternativaModels { get; set; } = new List<RespostaModel>();

    [JsonIgnore]
    public virtual List<AlternativaModel> AlternativaModels { get; set; } = new List<AlternativaModel>();
    
    public QuestaoModel(){}
    
    public QuestaoModel(QuestaoRequestDTO requestDto, CheckListModel checkList)
    {
        this.CheckListId = requestDto.CheckListId;
        
        this.Titulo = requestDto.Titulo;
        
        this.Tipo = requestDto.Tipo;
        
        this.CheckList = checkList;
        
    }
    
}
