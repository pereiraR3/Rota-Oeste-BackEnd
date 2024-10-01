namespace api_rota_oeste.Models.Usuario;

public record UsuarioPatchDTO
{
    public int Id { get; init; }
    public string? Telefone { get; init; }
    public string? Nome { get; init; }
    public byte[]? Foto { get; init; }
}