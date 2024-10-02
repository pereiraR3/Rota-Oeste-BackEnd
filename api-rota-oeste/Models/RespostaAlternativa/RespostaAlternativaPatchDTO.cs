namespace api_rota_oeste.Models.RespostaAlternativa;

/**
 * O atributo alternativa serve para identificar as possíveis alternativas/resposta do usuário
 * , e caso o valor for 0 indique que se trata de um upload de imagem 
 */
public record RespostaAlternativaPatchDTO(

    int Id,
    
    int? Alternativa,
    
    byte[]? Foto
    
);