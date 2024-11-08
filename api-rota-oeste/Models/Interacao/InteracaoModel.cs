using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.RespostaAlternativa;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Models.Interacao;

/// <summary>
/// Representa a classe <see cref="InteracaoModel"/> que descreve uma interação do sistema.
/// </summary>
/// <remarks>
/// Esta classe é responsável por armazenar as informações de uma interação realizada, que pode envolver um cliente respondendo a um checklist, incluindo o relacionamento com cliente e resposta.
/// </remarks>
/// <example>
/// Aqui está um exemplo de como usar a <see cref="InteracaoModel"/>:
/// <code>
/// var interacao = new InteracaoModel(cliente, checklist);
/// interacao.Data = DateTime.Now;
/// </code>
/// </example>
/// <seealso cref="ClienteModel"/>
/// <seealso cref="CheckListModel"/>
[Table("interacao")]
[Index(nameof(ClienteId), nameof(CheckListId), IsUnique = true)]
public class InteracaoModel
{
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "ClienteId é necessário")]
    [Column("id_cliente")]
    public int ClienteId { get; set; }

    [Required(ErrorMessage = "CheckListId é necessário")]
    [Column("id_checklist")]
    public int CheckListId { get; set; }
    
    [Required(ErrorMessage = "Status é necessário")]
    [Column("status", TypeName = "BIT")]
    public bool Status { get; set; }

    [DataType(DataType.Date)]
    [Column("data_criacao", TypeName = "DATETIME2")]
    public DateTime Data { get; set; }
    
    [JsonIgnore]
    [ForeignKey("ClienteId")]
    public virtual ClienteModel? Cliente { get; set; }
    
    [JsonIgnore]
    [ForeignKey("CheckListId")]
    public virtual CheckListModel? CheckList { get; set; }
    
    [JsonIgnore]
    public virtual List<RespostaModel> RespostaModels { get; set; } = new List<RespostaModel>();
    
    public InteracaoModel() {}

    public InteracaoModel(InteracaoRequestDTO request, ClienteModel cliente, CheckListModel checkList)
    {
        this.Status = request.Status;
        
        this.Data = DateTime.Now;
        
        this.Cliente = cliente;
        
        this.ClienteId = cliente.Id;
        
        this.CheckListId = request.CheckListId;
        
        this.CheckList = checkList;
        
    }
}