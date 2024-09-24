using api_rota_oeste.Models.Usuario;

namespace api_rota_oeste.Repositories.Interfaces;

/**
 * Interface que serve para definir os metódos que irão operar na camada de persistência
 */
public interface IUsuarioRepository
{
    
    Task<UsuarioResponseDTO> Adicionar(UsuarioRequestDTO request);

    Task<UsuarioModel> BuscaPorId(int id);
    
    Task<bool> Apagar(int id);
    
}