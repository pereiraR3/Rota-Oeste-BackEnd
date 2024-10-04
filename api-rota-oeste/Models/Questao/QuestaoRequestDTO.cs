
namespace api_rota_oeste.Models.Questao;
public record QuestaoRequestDTO(
    
    int CheckListId,
    
    string Titulo,
    
    TipoQuestao Tipo
    
);
