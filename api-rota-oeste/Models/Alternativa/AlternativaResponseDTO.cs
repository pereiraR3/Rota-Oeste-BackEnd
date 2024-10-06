using api_rota_oeste.Models.Questao;
using api_rota_oeste.Models.RespostaTemAlternativa;

namespace api_rota_oeste.Models.Alternativa;

/// <summary>
/// Representa o DTO <see cref="AlternativaResponseDTO"/> que descreve os dados de resposta de uma alternativa.
/// </summary>
/// <remarks>
/// Esta classe é utilizada para transferir dados detalhados de uma alternativa, incluindo seu identificador, descrição, código, e as relações com a questão associada e respostas que possuem essa alternativa.
/// </remarks>
/// <example>
/// Aqui está um exemplo de como usar o <see cref="AlternativaResponseDTO"/>:
/// <code>
/// var alternativaResponse = new AlternativaResponseDTO(1, 1, "Descrição da alternativa", 2, questaoResponse, respostaTemAlternativaResponses);
/// </code>
/// </example>
/// <seealso cref="QuestaoResponseDTO"/>
/// <seealso cref="RespostaTemAlternativaResponseDTO"/>
public class AlternativaResponseDTO
{
    public int Id { get; set; }
    public int QuestaoId { get; set; }
    public string Descricao { get; set; }
    public int Codigo { get; set; }
    public QuestaoResponseDTO? Questao { get; set; }
    public List<RespostaTemAlternativaResponseDTO>? RespostaTemAlternativaResponseDtos { get; set; }

    // Construtor sem argumentos
    public AlternativaResponseDTO() { }

    // Construtor completo (opcional)
    public AlternativaResponseDTO(int id, int questaoId, string descricao, int codigo, 
        QuestaoResponseDTO? questao, 
        List<RespostaTemAlternativaResponseDTO>? respostaTemAlternativaResponseDtos)
    {
        Id = id;
        QuestaoId = questaoId;
        Descricao = descricao;
        Codigo = codigo;
        Questao = questao;
        RespostaTemAlternativaResponseDtos = respostaTemAlternativaResponseDtos;
    }
}
