using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Cliente;

namespace api_rota_oeste.Models.ClienteRespondeCheckList;

public record ClienteRespondeCheckListResponseDTO(
    
    int ClienteId,
    
    int CheckListId,
    
    ClienteResponseDTO? Cliente,
    
    CheckListResponseDTO? CheckList
    
);