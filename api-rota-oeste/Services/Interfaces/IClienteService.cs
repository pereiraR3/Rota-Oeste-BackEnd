using api_rota_oeste.Models.Cliente;

namespace api_rota_oeste.Services.Interfaces;

public interface IClienteService
{
    Task<ClienteResponseMinDTO> AdicionarAsync(ClienteRequestDTO request);
  
    Task<List<ClienteResponseDTO>> AdicionarColecaoAsync(ClienteCollectionDTO request);

    Task<ClienteResponseMinDTO?> BuscarPorIdAsync(int id);
  
    Task<List<ClienteResponseMinDTO>> BuscarTodosAsync();
  
    Task<bool> ApagarAsync(int id);

    Task<bool> ApagarTodosAsync();
    
}