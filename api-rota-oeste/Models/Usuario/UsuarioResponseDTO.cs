using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Cliente;

namespace api_rota_oeste.Models.Usuario
{
    public record UsuarioResponseDTO
    (   
        int Id,
        
        string Telefone,
        
        string Nome, 
        
        byte[] Foto,
        
        List<ClienteResponseDTO>? Clientes,
        
        List<CheckListResponseDTO>? CheckLists
        
    );
}