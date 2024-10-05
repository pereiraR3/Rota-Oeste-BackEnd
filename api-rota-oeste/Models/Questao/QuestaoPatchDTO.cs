namespace api_rota_oeste.Models.Questao;

public record QuestaoPatchDTO(
    
    int Id,
    
    string? Titulo,
    
    TipoQuestao? Tipo

);