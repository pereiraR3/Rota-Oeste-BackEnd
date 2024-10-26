namespace api_rota_oeste.Models.Cliente;

public record ClienteResponseMinDTO
{
    public int Id { get; set; }
    
    public int UsuarioId { get; set; }
    
    public string Nome { get; set; }
    
    public string Telefone { get; set; }
    
    public byte[]? Foto { get; set; } 
    
    public ClienteResponseMinDTO(int id, int usuarioId, string nome, string telefone, byte[]? foto = null)
    {
        Id = id;
        UsuarioId = usuarioId;
        Nome = nome;
        Telefone = telefone;
        Foto = foto;
    }
    
}