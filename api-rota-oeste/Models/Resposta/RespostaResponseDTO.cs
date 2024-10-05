using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Models.RespostaTemAlternativa;

namespace api_rota_oeste.Models.RespostaAlternativa;

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
