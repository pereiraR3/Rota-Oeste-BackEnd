
using api_rota_oeste.Models.Alternativa;
using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.RespostaAlternativa;
namespace api_rota_oeste.Models.Questao;

/// <summary>
/// Representa o DTO <see cref="QuestaoResponseDTO"/> que descreve os dados de resposta de uma questão.
/// </summary>
/// <remarks>
/// Este DTO é utilizado para transferir informações detalhadas de uma questão, incluindo o identificador, título, tipo, checklist associado, respostas e alternativas associadas.
/// </remarks>
/// <example>
/// Aqui está um exemplo de como usar o <see cref="QuestaoResponseDTO"/>:
/// <code>
/// var questaoResponse = new QuestaoResponseDTO(1, 1, "Título da Questão", TipoQuestao.QUESTAO_MULTIPLA_ESCOLHA, checkListResponse, respostas, alternativas);
/// </code>
/// </example>
/// <seealso cref="CheckListResponseDTO"/>
/// <seealso cref="RespostaResponseDTO"/>
/// <seealso cref="AlternativaResponseDTO"/>
public record QuestaoResponseDTO(
    
    int Id,
    
    int CheckListId,
    
    string Titulo,
    
    TipoQuestao Tipo,
    
    CheckListResponseDTO? CheckList,
    
    List<RespostaResponseDTO>? RespostaModels,
    
    List<AlternativaResponseDTO>? AlternativaModels
    
);
