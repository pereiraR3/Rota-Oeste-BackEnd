
namespace api_rota_oeste.Models.RespostaAlternativa;
public record RespostaAlternativaRequestDTO
(
    
    int QuestaoId,
    
    int InteracaoId,
    
    int? Alternativa,
    
    byte[]? Foto
    
);
