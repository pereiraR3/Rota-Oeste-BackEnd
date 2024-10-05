namespace api_rota_oeste.Models.RespostaAlternativa;

/// <summary>
/// DTO para atualização parcial de uma resposta alternativa.
/// O atributo foto serve para identificar um tipo de resposta que poderá ser dada pelo usuário
/// 
public record RespostaPatchDTO(

    int Id,
    
    byte[]? Foto
    
);