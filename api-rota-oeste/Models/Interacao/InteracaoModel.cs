using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Cliente;

namespace api_rota_oeste.Models.Interacao;

[Table("interacao")]
public class InteracaoModel
{
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "ClienteId é necessário")]
    public int ClienteId { get; set; }

    [Required(ErrorMessage = "CheckListId é necessário")]
    public int CheckListId { get; set; }
    
    [Required(ErrorMessage = "Status é necessário")]
    [Column(TypeName = "BIT")]
    public bool Status { get; set; }

    [Column(TypeName = "DATETIME2")]
    public DateTime Data { get; set; }
    
    [JsonIgnore]
    [ForeignKey("ClienteId")]
    public virtual ClienteModel? Cliente { get; set; }
    
    [JsonIgnore]
    [ForeignKey("CheckListId")]
    public virtual CheckListModel? CheckList { get; set; }
    
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