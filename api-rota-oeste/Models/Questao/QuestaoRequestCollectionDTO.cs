using api_rota_oeste.Models.Alternativa;

namespace api_rota_oeste.Models.Questao;

public record QuestaoRequestCollectionDTO(
    
    QuestaoRequestDTO Questao,
    
    List<AlternativaRequestDTO> Alternativas
    
);