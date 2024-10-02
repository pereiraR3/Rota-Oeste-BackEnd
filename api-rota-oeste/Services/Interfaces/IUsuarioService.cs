using api_rota_oeste.Models.Usuario;

namespace api_rota_oeste.Services.Interfaces;

public interface IUsuarioService
{
    Task<UsuarioResponseDTO> AdicionarAsync(UsuarioRequestDTO request);

    Task<UsuarioResponseDTO> BuscarPorIdAsync(int id);
    
    Task<List<UsuarioResponseDTO>> BuscarTodosAsync();
    
    Task<bool> AtualizarAsync(UsuarioPatchDTO request);
    
    Task<bool> ApagarAsync(int id);
}