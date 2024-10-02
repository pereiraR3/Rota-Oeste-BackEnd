
namespace api_rota_oeste.Models.Cliente;

public record ClienteRequestDTO
(
    
    int UsuarioId,
    
    string Nome,
    
    string Telefone, 
    
    byte[]? Foto
    
);
