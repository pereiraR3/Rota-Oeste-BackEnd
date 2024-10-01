using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Models.Usuario;

namespace api_rota_oeste.Models.CheckList;

[Table("checklist")]
public class CheckListModel
{
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "UsuarioId é necessário")]
    public int UsuarioId { get; set; }
    
    [Required(ErrorMessage = "Nome é necessário")]
    [StringLength(60, ErrorMessage = "O nome não pode exceder 60 caracteres")]
    public string? Nome { get; set; }
    
    [Required(ErrorMessage = "Data de Criação é necessária")]
    [DataType(DataType.Date)]
    public DateTime? DataCriacao { get; set; }
    
    [JsonIgnore]
    [ForeignKey("UsuarioId")]
    public virtual UsuarioModel Usuario { get; set; }

    [JsonIgnore] public virtual List<QuestaoModel> Questoes { get; set; } = new List<QuestaoModel>();

    public CheckListModel() { }
    
    public CheckListModel(CheckListRequestDTO checkListRequestDto, UsuarioModel usuario) {
        
        this.Nome = checkListRequestDto.Nome;
        
        this.DataCriacao = DateTime.Now; 
        
        this.Usuario = usuario;
        
        this.UsuarioId = checkListRequestDto.UsuarioId;
            
    }
    
}
