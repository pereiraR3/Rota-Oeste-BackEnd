using api_rota_oeste.Models.Usuario;

namespace api_rota_oeste.Repositories.Interfaces;

/**
 * Interface que serve para definir os metódos que irão operar na camada de persistência
 * em relação à entidade Usuario
 */
public interface IUsuarioRepository
{
    
    Task<UsuarioModel?> Adicionar(UsuarioModel usuario);

    Task<UsuarioModel?> BuscaPorId(int id);
    
    Task<bool> Apagar(int id);


}