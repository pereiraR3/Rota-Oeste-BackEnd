using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using api_rota_oeste.Models.Usuario;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Models.Cliente;

[Table("cliente")]
[Index(nameof(Telefone), IsUnique = true)]
public sealed class ClienteModel
{
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "UsuarioId é necessário")]
    [Column("id_usuario")]
    public int UsuarioId { get; set; }
    
    [Required(ErrorMessage = "Nome não pode exceder 60 caracteres")]
    [StringLength(60, ErrorMessage = "")]
    public string Nome { get; set; }
    
    [Required(ErrorMessage = "Telefone é necessário")]
    [StringLength(11, ErrorMessage = "O telefone não pode exceder 11 caracteres")]
    public string Telefone { get; set; }
    
    public byte[]? Foto { get; set; }
    
    [JsonIgnore]
    [ForeignKey("UsuarioId")]
    public UsuarioModel? Usuario { get; set; }
    
    public ClienteModel(){}

    public ClienteModel(ClienteRequestDTO request, UsuarioModel usuario)
    {
        this.UsuarioId = request.UsuarioId;
        this.Usuario = usuario;
        this.Nome = request.Nome;
        this.Telefone = request.Telefone;
        this.Foto = request.Foto;
    }
    
}
