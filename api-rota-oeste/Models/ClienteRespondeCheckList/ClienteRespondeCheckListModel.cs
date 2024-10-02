using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Cliente;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Models.ClienteRespondeCheckList;

[Table("cliente_responde_checklist")]
[Index(nameof(ClienteId), nameof(CheckListId), IsUnique = true)]
public class ClienteRespondeCheckListModel
{
    [Column("id_cliente")]
    public int ClienteId { get; set; }
    
    [Column("id_checklist")]
    public int CheckListId { get; set; }
    
    [JsonIgnore]
    public virtual ClienteModel Cliente { get; set; }
    
    [JsonIgnore]
    public virtual CheckListModel CheckList { get; set; }

    public ClienteRespondeCheckListModel(){}
    
    public ClienteRespondeCheckListModel(
        int clienteId,
        int checkListId,
        ClienteModel cliente,
        CheckListModel checkListModel
    )
    {
        this.ClienteId = clienteId;
        this.CheckListId = checkListId;
        this.Cliente = cliente;
        this.CheckList = checkListModel;
    }
}