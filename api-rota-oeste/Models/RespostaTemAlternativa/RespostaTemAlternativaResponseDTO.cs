using api_rota_oeste.Models.Alternativa;
using api_rota_oeste.Models.RespostaAlternativa;

namespace api_rota_oeste.Models.RespostaTemAlternativa;

public class RespostaTemAlternativaResponseDTO
{
    public int RespostaId { get; set; }
    public int AlternativaId { get; set; }
    public RespostaResponseDTO? Resposta { get; set; }
    public AlternativaResponseDTO? Alternativa { get; set; }

    // Construtor sem argumentos
    public RespostaTemAlternativaResponseDTO() { }

    // Construtor completo (opcional)
    public RespostaTemAlternativaResponseDTO(int respostaId, int alternativaId, RespostaResponseDTO? resposta, AlternativaResponseDTO? alternativa)
    {
        RespostaId = respostaId;
        AlternativaId = alternativaId;
        Resposta = resposta;
        Alternativa = alternativa;
    }
}
