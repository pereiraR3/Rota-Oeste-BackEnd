using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.ClienteRespondeCheckList;
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Models.Usuario;

namespace api_rota_oeste.Models.CheckList;

[Table("checklist")]
public class CheckListModel
{
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "UsuarioId é necessário")]
    [Column("id_usuario")]
    public int UsuarioId { get; set; }
    
    [Required(ErrorMessage = "Nome é necessário")]
    [StringLength(60, ErrorMessage = "O nome não pode exceder 60 caracteres")]
    [Column("nome")]
    public string? Nome { get; set; }
    
    [Required(ErrorMessage = "Data de Criação é necessária")]
    [DataType(DataType.Date)]
    [Column("data_criacao")]
    public DateTime? DataCriacao { get; set; }
    
    [JsonIgnore]
    [ForeignKey("UsuarioId")]
    public virtual UsuarioModel Usuario { get; set; }

    [JsonIgnore]
    public virtual List<QuestaoModel>? Questoes { get; set; } = new List<QuestaoModel>();
    
    [JsonIgnore]
    public virtual List<ClienteRespondeCheckListModel>? ClienteRespondeCheckLists { get; set; } = new List<ClienteRespondeCheckListModel>();

    public CheckListModel() { }
    
    public CheckListModel(CheckListRequestDTO checkListRequestDto, UsuarioModel usuario) {
        
        this.Nome = checkListRequestDto.Nome;
        
        this.DataCriacao = DateTime.Now; 
        
        this.Usuario = usuario;
        
        this.UsuarioId = checkListRequestDto.UsuarioId;
            
    }
    
}
