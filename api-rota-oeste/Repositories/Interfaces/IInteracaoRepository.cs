using api_rota_oeste.Models.Interacao;

namespace api_rota_oeste.Repositories.Interfaces;

/**
 * Interface que serve para definir os metódos que irão operar na camada de persistência
 * em relação à entidade Interacao
 */
public interface IInteracaoRepository {
    
    Task<InteracaoModel?> Adicionar(InteracaoModel interacaoModel);
    
    Task<InteracaoModel?> BuscarPorId(int id);
    
    Task<List<InteracaoModel>> BuscarTodos();
    
    Task<bool> ApagarPorId(int id);

    Task<bool> ApagarTodos();

    Task<InteracaoModel?> BuscarPorIdCliente(int clienteExistenteId);
}