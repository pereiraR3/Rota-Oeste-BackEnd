namespace api_rota_oeste.Models.Resposta;

public record RespostaResponseMinDTO
{
    public int Id { get; set; }
    public int QuestaoId { get; set; }
    public int InteracaoId { get; set; }
    public byte[]? Foto { get; set; }

    // Construtor com todos os argumentos (opcional)
    public RespostaResponseMinDTO(
        
        int id,
        int questaoId,
        int interacaoId,
        byte[]? foto
        
    )
    {
        Id = id;
        QuestaoId = questaoId;
        InteracaoId = interacaoId;
        Foto = foto;
    }
}