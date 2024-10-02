using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Models.Questao;

namespace api_rota_oeste.Models.RespostaAlternativa;

public record RespostaAlternativaResponseDTO(
    
    int Id, 
    
    int QuestaoId,
    
    int InteracaoId,
    
    int? Alternativa,
    
    byte[]? Foto,
    
    QuestaoModel? Questao,
    
    InteracaoModel? Interacao
    
);