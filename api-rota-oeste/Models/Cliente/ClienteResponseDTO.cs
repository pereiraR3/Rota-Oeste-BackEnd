
namespace api_rota_oeste.Models.Cliente;

public record ClienteResponseDTO
(
    
    int Id,
    
    int UsuarioId,
    
    string Nome,
    
    string Telefone, 
    
    byte[] Foto
    
);
