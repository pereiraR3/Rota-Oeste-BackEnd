using api_rota_oeste.Models.Cliente;

namespace api_rota_oeste.Repositories.Interfaces;

/**
 * Interface que serve para definir os metódos que irão operar na camada de persistência
 */
public interface IClienteRepository
{
 
  Task<ClienteModel> Adicionar(ClienteRequestDTO request);
  
  Task<List<ClienteModel>> AdicionarColecao(ClienteCollectionDTO request);

  Task<ClienteModel> BuscaPorId(int id);
  
  Task<List<ClienteModel>> BuscaTodos();
  
  Task<bool> Apagar(int id);

  Task<bool> ApagarTodos();

}