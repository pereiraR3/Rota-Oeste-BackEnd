
namespace api_rota_oeste.Models.RespostaAlternativa;

/// <summary>
/// Representa o DTO <see cref="RespostaRequestDTO"/> que descreve os campos necessários para a criação de uma nova resposta alternativa.
/// </summary>
/// <remarks>
/// Este DTO é utilizado para transferir os dados ao criar uma nova resposta, incluindo o identificador da questão, o identificador da interação e uma foto associada como resposta (opcional).
/// </remarks>
/// <example>
/// Aqui está um exemplo de como usar o <see cref="RespostaRequestDTO"/>:
/// <code>
/// var respostaRequest = new RespostaRequestDTO(1, 2, fotoBytes);
/// </code>
/// </example>
/// <seealso cref="QuestaoId"/>
/// <seealso cref="InteracaoId"/>
public record RespostaRequestDTO
(
    
    int QuestaoId,
    
    int InteracaoId,
    
    byte[]? Foto
    
);
