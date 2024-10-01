using api_rota_oeste.Models.Cliente;

namespace api_rota_oeste.Services.Interfaces;

public interface IClienteService
{
    Task<ClienteResponseDTO> AdicionarAsync(ClienteRequestDTO request);
  
    Task<List<ClienteResponseDTO>> AdicionarColecaoAsync(ClienteCollectionDTO request);

    Task<ClienteResponseDTO?> BuscarPorIdAsync(int id);
  
    Task<List<ClienteResponseDTO>> BuscarTodosAsync();
  
    Task<bool> ApagarAsync(int id);

    Task<bool> ApagarTodosAsync();
    
}