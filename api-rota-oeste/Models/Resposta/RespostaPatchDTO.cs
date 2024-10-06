namespace api_rota_oeste.Models.RespostaAlternativa;

/// <summary>
/// Representa o DTO <see cref="RespostaPatchDTO"/> que descreve os campos para atualização parcial de uma resposta alternativa.
/// </summary>
/// <remarks>
/// Este DTO é utilizado para transferir dados ao atualizar parcialmente uma resposta, incluindo o identificador e a foto associada à resposta.
/// O atributo Foto é opcional e serve para identificar um tipo de resposta que poderá ser dada pelo usuário.
/// </remarks>
/// <example>
/// Aqui está um exemplo de como usar o <see cref="RespostaPatchDTO"/>:
/// <code>
/// var respostaPatch = new RespostaPatchDTO(1, fotoBytes);
/// </code>
/// </example>
public record RespostaPatchDTO(

    int Id,
    
    byte[]? Foto
    
);