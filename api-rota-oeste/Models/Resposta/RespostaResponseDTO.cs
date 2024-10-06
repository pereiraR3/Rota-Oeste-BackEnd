using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Models.RespostaTemAlternativa;

namespace api_rota_oeste.Models.RespostaAlternativa;

/// <summary>
/// Representa o DTO <see cref="RespostaResponseDTO"/> que descreve os dados de resposta de uma resposta alternativa.
/// </summary>
/// <remarks>
/// Este DTO é utilizado para transferir informações detalhadas de uma resposta, incluindo identificadores da questão e da interação, uma foto associada, a questão e interação relacionadas, e uma lista de alternativas da resposta.
/// </remarks>
/// <example>
/// Aqui está um exemplo de como usar o <see cref="RespostaResponseDTO"/>:
/// <code>
/// var respostaResponse = new RespostaResponseDTO(1, 1, 2, fotoBytes, questaoResponse, interacaoResponse, respostaTemAlternativaResponseDtos);
/// </code>
/// </example>
/// <seealso cref="QuestaoResponseDTO"/>
/// <seealso cref="InteracaoResponseDTO"/>
/// <seealso cref="RespostaTemAlternativaResponseDTO"/>
public class RespostaResponseDTO
{
    public int Id { get; set; }
    public int QuestaoId { get; set; }
    public int InteracaoId { get; set; }
    public byte[]? Foto { get; set; }
    public QuestaoResponseDTO? Questao { get; set; }
    public InteracaoResponseDTO? Interacao { get; set; }
    public List<RespostaTemAlternativaResponseDTO> RespostaTemAlternativaResponseDtos { get; set; }

    // Construtor sem argumentos
    public RespostaResponseDTO()
    {
        RespostaTemAlternativaResponseDtos = new List<RespostaTemAlternativaResponseDTO>();
    }

    // Construtor com todos os argumentos (opcional)
    public RespostaResponseDTO(
        
        int id,
        int questaoId,
        int interacaoId,
        byte[]? foto, 
        QuestaoResponseDTO? questao,
        InteracaoResponseDTO? interacao, 
        List<RespostaTemAlternativaResponseDTO> respostaTemAlternativaResponseDtos
        
        )
    {
        Id = id;
        QuestaoId = questaoId;
        InteracaoId = interacaoId;
        Foto = foto;
        Questao = questao;
        Interacao = interacao;
        RespostaTemAlternativaResponseDtos = respostaTemAlternativaResponseDtos;
    }
}
