
namespace api_rota_oeste.Models.Questao;
/// <summary>
/// Representa o DTO <see cref="QuestaoRequestDTO"/> que descreve os campos necessários para a criação de uma nova questão.
/// </summary>
/// <remarks>
/// Este DTO é utilizado para transferir dados ao criar uma nova questão, incluindo o identificador do checklist, o título e o tipo da questão.
/// </remarks>
/// <example>
/// Aqui está um exemplo de como usar o <see cref="QuestaoRequestDTO"/>:
/// <code>
/// var questaoRequest = new QuestaoRequestDTO(1, "Título da Questão", TipoQuestao.QUESTAO_MULTIPLA_ESCOLHA);
/// </code>
/// </example>
/// <seealso cref="TipoQuestao"/>
public record QuestaoRequestDTO(
    
    int CheckListId,
    
    string Titulo,
    
    TipoQuestao Tipo
    
);
