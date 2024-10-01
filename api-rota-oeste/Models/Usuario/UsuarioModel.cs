using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Cliente;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Models.Usuario;

[Table("usuario")]
[Index(nameof(Telefone), IsUnique = true)]
public class UsuarioModel
{
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
        
    [Required(ErrorMessage = "Telefone é necessário")]
    [StringLength(11, ErrorMessage = "O telefone não pode exceder 11 caracteres")]
    public string Telefone { get; set; }
        
    [Required(ErrorMessage = "Nome é necessário")]
    [StringLength(60, ErrorMessage = "O nome não pode exceder 60 caracteres")]
    public string Nome { get; set; }
        
    [JsonIgnore]
    [Required(ErrorMessage = "Senha é necessária")]
    [StringLength(60, MinimumLength = 8, ErrorMessage = "A senha não pode exceder a 60 caracteres")]
    public string Senha { get; set; }
    public byte[]? Foto { get; set; }
        
    [JsonIgnore]
    public virtual List<ClienteModel> Clientes { get; set; } = new List<ClienteModel>();
    
    [JsonIgnore]
    public virtual List<CheckListModel> CheckLists { get; set; } = new List<CheckListModel>();

    public UsuarioModel(){}
        
    public UsuarioModel(UsuarioRequestDTO request)
    {
        this.Telefone = request.Telefone;
        this.Nome = request.Nome;
        this.Senha = request.Senha;
        this.Foto = request.Foto;
    }
        
}
