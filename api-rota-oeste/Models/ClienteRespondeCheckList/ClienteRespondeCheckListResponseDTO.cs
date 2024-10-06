using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Cliente;

namespace api_rota_oeste.Models.ClienteRespondeCheckList;

/// <summary>
/// Representa o DTO <see cref="ClienteRespondeCheckListResponseDTO"/> que descreve os dados de resposta de um relacionamento entre cliente e checklist.
/// </summary>
/// <remarks>
/// Este DTO é utilizado para transferir informações detalhadas sobre a relação entre um cliente e um checklist, incluindo os identificadores e os dados dos respectivos cliente e checklist.
/// </remarks>
/// <example>
/// Aqui está um exemplo de como usar o <see cref="ClienteRespondeCheckListResponseDTO"/>:
/// <code>
/// var clienteRespondeCheckListResponse = new ClienteRespondeCheckListResponseDTO(1, 2, clienteResponse, checkListResponse);
/// </code>
/// </example>
/// <seealso cref="ClienteResponseDTO"/>
/// <seealso cref="CheckListResponseDTO"/>
public record ClienteRespondeCheckListResponseDTO(
    
    int ClienteId,
    
    int CheckListId,
    
    ClienteResponseDTO? Cliente,
    
    CheckListResponseDTO? CheckList
    
);