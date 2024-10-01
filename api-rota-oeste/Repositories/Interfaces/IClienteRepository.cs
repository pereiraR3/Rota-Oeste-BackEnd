using api_rota_oeste.Models.Cliente;

namespace api_rota_oeste.Repositories.Interfaces;

/**
 * Interface que serve para definir os metódos que irão operar na camada de persistência
 * em relação à entidade Cliente
 */
public interface IClienteRepository
{
 
  Task<ClienteModel> Adicionar(ClienteModel clienteModel);
  
  Task<ClienteModel?> BuscarPorId(int id);
  
  Task<List<ClienteModel>> BuscarTodos();
  
  Task<bool> Apagar(int id);

  Task<bool> ApagarTodos();

}