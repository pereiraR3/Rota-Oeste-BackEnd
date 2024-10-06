
namespace api_rota_oeste.Models.Interacao;

/// <summary>
/// Representa o DTO <see cref="InteracaoRequestDTO"/> que descreve os campos necessários para a criação de uma nova interação.
/// </summary>
/// <remarks>
/// Este DTO é utilizado para transferir dados ao criar uma nova interação, incluindo o identificador do cliente, o identificador do checklist, e o status da interação.
/// </remarks>
/// <example>
/// Aqui está um exemplo de como usar o <see cref="InteracaoRequestDTO"/>:
/// <code>
/// var interacaoRequest = new InteracaoRequestDTO(1, 2, true);
/// </code>
/// </example>
public record InteracaoRequestDTO(
    
    int ClienteId,
    
    int CheckListId,
    
    bool Status
    
);
