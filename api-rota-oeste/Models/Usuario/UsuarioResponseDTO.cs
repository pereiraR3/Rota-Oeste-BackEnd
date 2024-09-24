namespace api_rota_oeste.Models.Usuario
{
    public record UsuarioResponseDTO
    (   
        int Id,
        
        string Telefone,
        
        string Nome, 
        
        byte[] Foto
        
    );
}