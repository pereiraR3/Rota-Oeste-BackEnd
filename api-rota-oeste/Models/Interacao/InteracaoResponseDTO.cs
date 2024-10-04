using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.RespostaAlternativa;

namespace api_rota_oeste.Models.Interacao;

public record InteracaoResponseDTO
(
    
    int Id,
    
    int ClienteId,
    
    int CheckListId,
    
    bool Status,
    
    ClienteResponseDTO? Cliente,
    
    CheckListResponseDTO? CheckList,
    
    List<RespostaResponseDTO>? RespostaAlternativaModels
    
);

