using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Repositories.Interfaces;
using AutoMapper;

namespace api_rota_oeste.Services.Interfaces;

/**
 * Representa a camada de serviço, isto é, a camada onde fica as regras de negócio da aplicação
 */
public class ClienteService : IClienteService
{
    
    private readonly IClienteRepository _clienteRepository;
    private readonly IMapper _mapper;

    public ClienteService(IClienteRepository clienteRepository, IMapper mapper)
    {
        _clienteRepository = clienteRepository;
        _mapper = mapper;
    }

    /**
     * Método da camada de servico -> para criar uma entidade do tipo cliente
     */
    public async Task<ClienteResponseDTO> AdicionarAsync(ClienteRequestDTO request)
    {
        ClienteModel cliente = await _clienteRepository.Adicionar(request);
        
        return _mapper.Map<ClienteResponseDTO>(cliente);
    }
    
    /**
     * Método da camada de serviço -> para criar em massa entidades do tipo cliente
     */
    public async Task<List<ClienteResponseDTO>> AdicionarColecaoAsync(ClienteCollectionDTO request)
    {
        List<ClienteModel> clientes = await _clienteRepository.AdicionarColecao(request);

        if (clientes == null || !clientes.Any())
            throw new InvalidOperationException("Conteúdo não encontrado");
        
        List<ClienteResponseDTO> clientesResponse = clientes
            .Select(i => _mapper.Map<ClienteResponseDTO>(i))
            .ToList();

        return clientesResponse;
    }

    /**
     * Método da camada de serviço -> para buscar uma entidade do tipo cliente pelo seu ID
     */
    public async Task<ClienteResponseDTO?> BuscaPorIdAsync(int id)
    {
        ClienteModel? cliente = await _clienteRepository.BuscaPorId(id);

        if (cliente == null)
            throw new KeyNotFoundException("Entidade cliente não encontrada");
        
        return _mapper.Map<ClienteResponseDTO>(cliente);
    }

    /**
     * Método da camada de serviço -> para buscar todas entidades do tipo cliente
     */
    public async Task<List<ClienteResponseDTO>> BuscaTodosAsync()
    {

        List<ClienteModel?> clienteModels = await _clienteRepository.BuscaTodos();
        
        if (clienteModels == null || !clienteModels.Any())
            throw new InvalidOperationException("Conteúdo não encontrado");
        
        List<ClienteResponseDTO> clientesResponse = clienteModels
            .Select(i => _mapper.Map<ClienteResponseDTO>(i))
            .ToList();
        
        return clientesResponse;
    }

    /**
     * Método da camada de serviço -> para buscar apagar uma entidade cliente pelo ID
     */
    public async Task<bool> ApagarAsync(int id)
    {
        var resultado = await _clienteRepository.Apagar(id);

        if (!resultado)
            throw new ApplicationException("Operação não foi realizada");
        
        return true;
    }

    /**
     * Método da camada de serviço -> para apagar em massa todas as entidades do tipo cliente
     */
    public async Task<bool> ApagarTodosAsync()
    {
        var resultado = await _clienteRepository.ApagarTodos();

        if (!resultado)
            throw new ApplicationException("Operação não foi realizada");
        
        return true;

    }
}