namespace api_rota_oeste.Models.Questao;

/// <summary>
/// Representa o DTO <see cref="QuestaoPatchDTO"/> que descreve os campos para atualização parcial de uma questão.
/// </summary>
/// <remarks>
/// Este DTO é utilizado para transferir dados ao atualizar parcialmente uma questão, incluindo o título e o tipo da questão.
/// </remarks>
/// <example>
/// Aqui está um exemplo de como usar o <see cref="QuestaoPatchDTO"/>:
/// <code>
/// var questaoPatch = new QuestaoPatchDTO(1, "Novo Título da Questão", TipoQuestao.QUESTAO_OBJETIVA);
/// </code>
/// </example>
/// <seealso cref="TipoQuestao"/>
public record QuestaoPatchDTO(
    
    int Id,
    
    string? Titulo,
    
    TipoQuestao? Tipo

);