namespace api_rota_oeste.Models.Interacao;

/// <summary>
/// Representa o DTO <see cref="InteracaoPatchDTO"/> que descreve os campos para atualização parcial de uma interação.
/// </summary>
/// <remarks>
/// Este DTO é utilizado para transferir dados ao atualizar parcialmente uma interação, incluindo o identificador da interação e o status (opcional).
/// </remarks>
/// <example>
/// Aqui está um exemplo de como usar o <see cref="InteracaoPatchDTO"/>:
/// <code>
/// var interacaoPatch = new InteracaoPatchDTO(1, true);
/// </code>
/// </example>
public record InteracaoPatchDTO(
    
    int Id,
    
    bool? Status
    
);

