
using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.RespostaAlternativa;

namespace api_rota_oeste.Models.Questao;
public record QuestaoResponseDTO(
    
    int Id,
    
    int CheckListId,
    
    string Titulo,
    
    string Tipo,
    
    CheckListModel? CheckList,
    
    List<RespostaAlternativaModel>? RespostaAlternativaModels
    
);
