namespace api_rota_oeste.Models.Usuario;

public record UsuarioRequestDTO(
    
    string Telefone, 
    
    string Nome,
    
    string Senha,
    
    byte[] Foto
    
);