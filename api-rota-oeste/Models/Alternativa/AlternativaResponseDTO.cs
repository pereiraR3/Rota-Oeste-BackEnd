using api_rota_oeste.Models.Questao;
using api_rota_oeste.Models.RespostaTemAlternativa;

namespace api_rota_oeste.Models.Alternativa;

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
