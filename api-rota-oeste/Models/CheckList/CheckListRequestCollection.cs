using api_rota_oeste.Models.Questao;

namespace api_rota_oeste.Models.CheckList;

public record CheckListRequestCollection(
    
    CheckListRequestDTO CheckList,
    
    List<QuestaoRequestCollectionDTO> Questoes
      
  );