using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Cliente;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Models.Usuario;

/// <summary>
/// Representa um usuário do sistema, contendo informações como telefone, nome, senha e foto.
/// </summary>
[Table("usuario")]
[Index(nameof(Telefone), IsUnique = true)]
public class UsuarioModel
{
    /// <summary>
    /// Identificador único do usuário.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }
    
    /// <summary>
    /// Telefone do usuário, deve ter no máximo 11 caracteres.
    /// </summary>
    [Required(ErrorMessage = "Telefone é necessário")]
    [StringLength(11, ErrorMessage = "O telefone não pode exceder 11 caracteres")]
    [Column("telefone")]
    public string Telefone { get; set; }
    
    /// <summary>
    /// Nome do usuário, deve ter no máximo 60 caracteres.
    /// </summary>
    [Required(ErrorMessage = "Nome é necessário")]
    [StringLength(60, ErrorMessage = "O nome não pode exceder 60 caracteres")]
    [Column("nome")]
    public string Nome { get; set; }
    
    /// <summary>
    /// Senha do usuário. Não deve ser serializada no JSON.
    /// </summary>
    [JsonIgnore]
    [Required(ErrorMessage = "Senha é necessária")]
    [StringLength(60, MinimumLength = 8, ErrorMessage = "A senha não pode exceder a 60 caracteres")]
    [Column("senha")]
    public string Senha { get; set; }
    
    /// <summary>
    /// Foto do usuário, armazenada como um array de bytes.
    /// </summary>
    [Column("foto", TypeName = "VARBINARY(MAX)")]
    public byte[]? Foto { get; set; }
    
    /// <summary>
    /// Lista de clientes associados ao usuário. Não deve ser serializada no JSON.
    /// </summary>
    [JsonIgnore]
    public virtual List<ClienteModel> Clientes { get; set; } = new List<ClienteModel>();
    
    /// <summary>
    /// Lista de checklists associados ao usuário. Não deve ser serializada no JSON.
    /// </summary>
    [JsonIgnore]
    public virtual List<CheckListModel> CheckLists { get; set; } = new List<CheckListModel>();

    /// <summary>
    /// Construtor padrão da classe <see cref="UsuarioModel"/>.
    /// </summary>
    public UsuarioModel() {}

    /// <summary>
    /// Construtor que inicializa um novo objeto <see cref="UsuarioModel"/> com base nos dados fornecidos em <see cref="UsuarioRequestDTO"/>.
    /// </summary>
    /// <param name="request">Objeto que contém as informações necessárias para criar um usuário.</param>
    public UsuarioModel(UsuarioRequestDTO request)
    {
        this.Telefone = request.Telefone;
        this.Nome = request.Nome;
        this.Senha = request.Senha;
        this.Foto = request.Foto;
    }
}
