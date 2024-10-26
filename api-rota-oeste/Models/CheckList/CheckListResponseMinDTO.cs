namespace api_rota_oeste.Models.CheckList;

public record CheckListResponseMinDTO
{
    public int Id { get; set; }
    
    public int UsuarioId { get; set; }
    
    public string Nome { get; set; }
    
    public DateTime? DataCriacao { get; set; }
    
    public int? QuantityQuestoes { get; set; }
    
    // Construtor com parâmetros
    public CheckListResponseMinDTO(int id, int usuarioId, string nome, DateTime? dataCriacao)
    {
        Id = id;
        UsuarioId = usuarioId;
        Nome = nome;
        DataCriacao = dataCriacao;
    }
}