using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.ClienteRespondeCheckList;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Models.Usuario;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Models.Cliente;

[Table("cliente")]
[Index(nameof(Telefone), IsUnique = true)]
public class ClienteModel
{
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "UsuarioId é necessário")]
    [Column("id_usuario")]
    public int UsuarioId { get; set; }
    
    [Required(ErrorMessage = "Nome não pode exceder 60 caracteres")]
    [StringLength(60, ErrorMessage = "O nome não pode exceder 60 caracteres")]
    [Column("nome")]
    public string Nome { get; set; }
    
    [Required(ErrorMessage = "Telefone é necessário")]
    [StringLength(11, ErrorMessage = "O telefone não pode exceder 11 caracteres")]
    [Column("telefone")]
    public string Telefone { get; set; }
    
    [Column("foto", TypeName = "VARBINARY(MAX)")]
    public byte[]? Foto { get; set; }
    
    [JsonIgnore]
    [ForeignKey("UsuarioId")]
    public virtual UsuarioModel Usuario { get; set; }

    [JsonIgnore]
    public virtual List<InteracaoModel>? Interacoes { get; set; } = new List<InteracaoModel>();
    
    [JsonIgnore]
    public virtual List<ClienteRespondeCheckListModel>? ClienteRespondeCheckLists { get; set; } = new List<ClienteRespondeCheckListModel>();
    
    public ClienteModel(){}

    public ClienteModel(ClienteRequestDTO request, UsuarioModel usuario)
    {
        UsuarioId = request.UsuarioId;
        
        Usuario = usuario;
        
        Nome = request.Nome;
        
        Telefone = request.Telefone;
        
        Foto = request.Foto;
    }
    
}
