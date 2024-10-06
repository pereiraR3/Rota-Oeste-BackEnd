namespace api_rota_oeste.Models.Alternativa;

/// <summary>
/// Representa o DTO <see cref="AlternativaPatchDTO"/> que descreve os campos para atualização parcial de uma alternativa.
/// </summary>
/// <remarks>
/// Este DTO é utilizado para transferir dados quando apenas alguns campos de uma alternativa precisam ser atualizados, como a descrição.
/// </remarks>
/// <example>
/// Aqui está um exemplo de como usar o <see cref="AlternativaPatchDTO"/>:
/// <code>
/// var alternativaPatch = new AlternativaPatchDTO(1, "Nova descrição da alternativa");
/// </code>
/// </example>
public record AlternativaPatchDTO(
    
    int Id,
    
    string? Descricao

);