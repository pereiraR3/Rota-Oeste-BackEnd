using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.ClienteRespondeCheckList;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Models.Usuario;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services.Interfaces;
using AutoMapper;

namespace api_rota_oeste.Services;

/**
 * Representa a camada de serviço, isto é, a camada onde fica as regras de negócio da aplicação
 */
public class ClienteService : IClienteService
{
    
    private readonly IClienteRepository _clienteRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IMapper _mapper;

    public ClienteService(
        
        IClienteRepository clienteRepository,
        IMapper mapper,
        IUsuarioRepository usuarioRepository
        
        )
    {
        _clienteRepository = clienteRepository;
        _usuarioRepository = usuarioRepository;
        _mapper = mapper;
    }

    /**
     * Método da camada de servico -> para criar uma entidade do tipo cliente
     */
    public async Task<ClienteResponseDTO> AdicionarAsync(ClienteRequestDTO clienteRequest)
    {

        UsuarioModel? usuarioModel = await _usuarioRepository.BuscaPorId(clienteRequest.UsuarioId);

        if (usuarioModel == null)
            throw new KeyNotFoundException("Usuário não encontrado");
        
        ClienteModel cliente = new ClienteModel(clienteRequest, usuarioModel);
        
        var clienteModel = await _clienteRepository.Adicionar(cliente);
        
        // Adicionando o Cliente à lista de clientes mapeados na entidade Usuario
        usuarioModel.Clientes.Add(cliente);
        
        // Refatorando a entidade para não puxar todas as suas associações com outras entidades
        clienteModel = RefatoraoMinClienteModel(clienteModel);
        
        return _mapper.Map<ClienteResponseDTO>(clienteModel);
    }
    
    /**
     * Método da camada de serviço -> para criar em massa entidades do tipo cliente
     */
    public async Task<List<ClienteResponseDTO>> AdicionarColecaoAsync(ClienteCollectionDTO clienteCollectionDto)
    {

        var usuarioModel = await _usuarioRepository.BuscaPorId(clienteCollectionDto.Clientes.First().UsuarioId);
        
        List<ClienteModel> clientes = new List<ClienteModel>();
        
        foreach (var cliente in clienteCollectionDto.Clientes)
        {
            ClienteModel clienteModel = new ClienteModel(cliente, usuarioModel);
            
            _clienteRepository.Adicionar(clienteModel);
            
            clientes.Add(clienteModel);
            
            usuarioModel.Clientes.Add(clienteModel);
            
        }
        
        List<ClienteResponseDTO> clientesResponse = clientes
            .Select(i => _mapper.Map<ClienteResponseDTO>(i))
            .ToList();

        return clientesResponse;
    }

    /**
     * Método da camada de serviço -> para buscar uma entidade do tipo cliente pelo seu ID
     */
    public async Task<ClienteResponseDTO?> BuscarPorIdAsync(int id)
    {
        
        if(id <= 0)
            throw new ArgumentException("O ID deve ser maior que zero.", nameof(id));
        
        ClienteModel? cliente = await _clienteRepository.BuscarPorId(id);

        if (cliente == null)
            throw new KeyNotFoundException("Entidade cliente não encontrada");
        
        cliente = RefatoraoMediumClienteModel(cliente);
        
        return _mapper.Map<ClienteResponseDTO>(cliente);
    }

    /**
     * Método da camada de serviço -> para buscar todas entidades do tipo cliente
     */
    public async Task<List<ClienteResponseDTO>> BuscarTodosAsync()
    {

        List<ClienteModel>? clienteModels = await _clienteRepository.BuscarTodos();
        
        List<ClienteResponseDTO> clientesResponse = clienteModels
            .Select(i => _mapper.Map<ClienteResponseDTO>(RefatoraoMinClienteModel(i)))
            .ToList();
        
        return clientesResponse;
    }

    /**
     * Método da camada de serviço -> para buscar apagar uma entidade cliente pelo ID
     */
    public async Task<bool> ApagarAsync(int id)
    {
        
        if(id <= 0)
            throw new ArgumentException("O ID deve ser maior que zero.", nameof(id));
        
        var resultado = await _clienteRepository.Apagar(id);

        if (!resultado)
            throw new KeyNotFoundException("Cliente não encontrado");
        
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

    /**
    * Método da camada de serviço -> para fazer a refatoracao de clienteModel, de modo que puxe apenas as
    * informações que foram julgadas como necessárias
    */
    public ClienteModel RefatoraoMinClienteModel(ClienteModel clienteModel)
    {
        
        clienteModel.ClienteRespondeCheckLists = new List<ClienteRespondeCheckListModel>();
        clienteModel.Interacoes = new List<InteracaoModel>();
        clienteModel.Usuario = new UsuarioModel
        {
            Id = clienteModel.Usuario.Id,
            Nome = clienteModel.Usuario.Nome,
            Telefone = clienteModel.Usuario.Telefone
        };

        return clienteModel;

    }
    
    /**
     * Método da camada de serviço -> para fazer a refatoracao de clienteModel, de modo que puxe apenas as
     * informações que foram julgadas como necessárias para o método getById
     */
    public ClienteModel RefatoraoMediumClienteModel(ClienteModel clienteModel)
    {
        if (clienteModel.ClienteRespondeCheckLists != null)
        {
            var clienteRespondeCheckListModels = clienteModel.ClienteRespondeCheckLists
                .Select(i => new ClienteRespondeCheckListModel
                {
                    ClienteId = i.ClienteId,
                    CheckListId = i.CheckListId
                }).ToList();
            
            clienteModel.ClienteRespondeCheckLists = clienteRespondeCheckListModels;
            
        }

        if (clienteModel.Interacoes != null)

        {
            var interacaoModels = clienteModel.Interacoes
                .Select(i => new InteracaoModel
                {
                    Id = i.Id,
                    ClienteId = i.ClienteId,
                    CheckListId = i.CheckListId,
                    Status = i.Status
                }).ToList();
            
            clienteModel.Interacoes = interacaoModels;
            
        }
        
        clienteModel.Usuario = new UsuarioModel
        {
            Id = clienteModel.Usuario.Id,
            Nome = clienteModel.Usuario.Nome,
            Telefone = clienteModel.Usuario.Telefone
        };

        return clienteModel;

    }
    
}