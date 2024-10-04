using api_rota_oeste.Models.Questao;

namespace api_rota_oeste.Models.Alternativa;

public record AlternativaResponseDTO(
    
    int Id,
    
    int QuestaoId,
    
    string Descricao,
    
    int Codigo,
    
    QuestaoModel? Questao

);