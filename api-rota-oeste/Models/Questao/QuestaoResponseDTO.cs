
using api_rota_oeste.Models.CheckList;

namespace api_rota_oeste.Models.Questao;
public record QuestaoResponseDTO(
    
    int Id,
    
    int CheckListId,
    
    string Titulo,
    
    string Tipo,
    
    CheckListModel? CheckList
    
);
