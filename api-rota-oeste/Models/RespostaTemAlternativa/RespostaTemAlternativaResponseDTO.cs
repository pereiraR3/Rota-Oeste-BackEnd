using api_rota_oeste.Models.Alternativa;
using api_rota_oeste.Models.Resposta;
using api_rota_oeste.Models.RespostaAlternativa;

namespace api_rota_oeste.Models.RespostaTemAlternativa;

/// <summary>
/// Representa o DTO <see cref="RespostaTemAlternativaResponseDTO"/> que descreve os dados de resposta do relacionamento entre uma resposta e uma alternativa.
/// </summary>
/// <remarks>
/// Este DTO é utilizado para transferir informações detalhadas do relacionamento entre uma resposta e uma alternativa, incluindo os identificadores da resposta e da alternativa, e suas respectivas representações detalhadas.
/// </remarks>
/// <example>
/// Aqui está um exemplo de como usar o <see cref="RespostaTemAlternativaResponseDTO"/>:
/// <code>
/// var respostaTemAlternativaResponse = new RespostaTemAlternativaResponseDTO(1, 2, respostaResponse, alternativaResponse);
/// </code>
/// </example>
/// <seealso cref="RespostaResponseMinDTO"/>
/// <seealso cref="AlternativaResponseMinDTO"/>
public class RespostaTemAlternativaResponseDTO
{
    public int RespostaId { get; set; }
    public int AlternativaId { get; set; }
    public RespostaResponseMinDTO? Resposta { get; set; }
    public AlternativaResponseMinDTO? Alternativa { get; set; }

    // Construtor sem argumentos
    public RespostaTemAlternativaResponseDTO() { }

    // Construtor completo (opcional)
    public RespostaTemAlternativaResponseDTO(int respostaId, int alternativaId, RespostaResponseMinDTO? resposta, AlternativaResponseMinDTO? alternativa)
    {
        RespostaId = respostaId;
        AlternativaId = alternativaId;
        Resposta = resposta;
        Alternativa = alternativa;
    }
}
