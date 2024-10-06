namespace api_rota_oeste.Models.Alternativa;

/// <summary>
/// Representa o DTO <see cref="AlternativaRequestDTO"/> que descreve os campos necessários para a criação de uma nova alternativa.
/// </summary>
/// <remarks>
/// Este DTO é utilizado para transferir dados ao criar uma nova alternativa, incluindo o identificador da questão associada e a descrição da alternativa.
/// </remarks>
/// <example>
/// Aqui está um exemplo de como usar o <see cref="AlternativaRequestDTO"/>:
/// <code>
/// var alternativaRequest = new AlternativaRequestDTO(1, "Descrição da nova alternativa");
/// </code>
/// </example>
public record AlternativaRequestDTO(
    
    int QuestaoId,
    
    string Descricao
    
);