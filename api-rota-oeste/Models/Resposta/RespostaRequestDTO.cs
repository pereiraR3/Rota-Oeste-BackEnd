
namespace api_rota_oeste.Models.RespostaAlternativa;
public record RespostaRequestDTO
(
    
    int QuestaoId,
    
    int InteracaoId,
    
    int Alternativa,
    
    byte[]? Foto
    
);
