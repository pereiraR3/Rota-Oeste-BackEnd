
namespace api_rota_oeste.Models.Interacao;

public record InteracaoRequestDTO(
    
    int ClienteId,
    
    int CheckListId,
    
    bool Status
    
);
