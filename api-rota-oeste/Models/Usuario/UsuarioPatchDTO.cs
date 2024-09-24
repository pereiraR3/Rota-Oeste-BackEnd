namespace api_rota_oeste.Models.Usuario;

public class UsuarioPatchDTO
{
    public int Id { get; set; }
    
    public string? Telefone { get; set; }
    
    public string? Nome { get; set; }
    
    public byte[]? Foto { get; set; }
    
}