
using api_rota_oeste.Models.Alternativa;
using api_rota_oeste.Models.CheckList;

namespace api_rota_oeste.Models.Questao;
public record QuestaoResponseDTO(
    
    int Id,
    
    int CheckListId,
    
    string Titulo,
    
    string Tipo,
    
    CheckListResponseDTO? CheckList,
    
    List<QuestaoResponseDTO>? RespostaAlternativaModels,
    
    List<AlternativaResponseDTO>? AlternativaModels
    
);
