using System.Text.Json.Serialization;
using api_rota_oeste.Models.CheckList;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_rota_oeste.Models.Questao;

[Table("questao")]
public class QuestaoModel{
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "CheckListId é necessário")]
    public int CheckListId { get; set; }
    
    [Required(ErrorMessage = "Titulo é necessário")]
    [StringLength(120, ErrorMessage = "O titulo não pode exceder 120 caracteres")]
    public string Titulo { get; set; }
    
    [Required(ErrorMessage = "Tipo é necessário")]
    [StringLength(20, ErrorMessage = "O tipo não pode exceder 20 caracteres")]
    public string Tipo { get; set; }
    
    [JsonIgnore]
    [ForeignKey("CheckListId")]
    public virtual CheckListModel? CheckList { get; set; }

    public QuestaoModel(){}
    
    public QuestaoModel(QuestaoRequestDTO requestDto, CheckListModel checkList)
    {
        this.CheckListId = requestDto.CheckListId;
        
        this.Titulo = requestDto.Titulo;
        
        this.Tipo = requestDto.Tipo;
        
        this.CheckList = checkList;
        
    }
    
}
