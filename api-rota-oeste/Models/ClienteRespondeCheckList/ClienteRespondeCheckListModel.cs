using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Cliente;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Models.ClienteRespondeCheckList;

/// <summary>
/// Representa a classe <see cref="ClienteRespondeCheckListModel"/> que descreve a relação entre um cliente e um checklist.
/// </summary>
/// <remarks>
/// Esta classe é responsável por armazenar as informações de relacionamento entre um cliente e um checklist, indicando quais checklists foram respondidos por quais clientes.
/// </remarks>
/// <example>
/// Aqui está um exemplo de como usar a <see cref="ClienteRespondeCheckListModel"/>:
/// <code>
/// var clienteRespondeCheckList = new ClienteRespondeCheckListModel(1, 2, clienteModel, checkListModel);
/// </code>
/// </example>
/// <seealso cref="ClienteModel"/>
/// <seealso cref="CheckListModel"/>
[Table("cliente_responde_checklist")]
[Index(nameof(ClienteId), nameof(CheckListId), IsUnique = true)]
public class ClienteRespondeCheckListModel
{
    [Column("id_cliente")]
    public int ClienteId { get; set; }
    
    [Column("id_checklist")]
    public int CheckListId { get; set; }
    
    [JsonIgnore]
    public virtual ClienteModel? Cliente { get; set; }
    
    [JsonIgnore]
    public virtual CheckListModel? CheckList { get; set; }

    public ClienteRespondeCheckListModel(){}
    
    public ClienteRespondeCheckListModel(
        int clienteId,
        int checkListId,
        ClienteModel? cliente,
        CheckListModel? checkListModel
    )
    {
        this.ClienteId = clienteId;
        this.CheckListId = checkListId;
        this.Cliente = cliente;
        this.CheckList = checkListModel;
    }
}