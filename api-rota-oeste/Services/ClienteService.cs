using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.ClienteRespondeCheckList;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Models.Usuario;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services.Interfaces;
using AutoMapper;

namespace api_rota_oeste.Services;

/// <summary>
/// Serviço responsável pelas operações de lógica de negócio relacionadas à entidade Cliente.
/// </summary>
/// <remarks>
/// Implementa a interface <see cref="IClienteService"/> e define métodos para adicionar, buscar, atualizar e apagar entidades do tipo Cliente.
/// Além disso, gerencia os relacionamentos entre Cliente e outras entidades, como Usuário, CheckList e Interacao.
/// </remarks>
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

    /// <summary>
    /// Cria uma nova entidade do tipo Cliente e a adiciona ao banco de dados.
    /// </summary>
    /// <param name="clienteRequest">Objeto contendo os dados do cliente a ser criado.</param>
    /// <returns>Retorna o DTO de resposta contendo as informações do cliente criado.</returns>
    /// <exception cref="KeyNotFoundException">Lançada se o usuário associado ao cliente não for encontrado.</exception>
    public async Task<ClienteResponseMinDTO> AdicionarAsync(ClienteRequestDTO clienteRequest)
    {

        UsuarioModel? usuarioModel = await _usuarioRepository.BuscaPorId(clienteRequest.UsuarioId);

        if (usuarioModel == null)
            throw new KeyNotFoundException("Usuário não encontrado");
        
        ClienteModel cliente = new ClienteModel(clienteRequest, usuarioModel);
        
        var clienteModel = await _clienteRepository.Adicionar(cliente);
        
        // Adicionando o Cliente à lista de clientes mapeados na entidade Usuario
        usuarioModel.Clientes.Add(cliente);
        
        return _mapper.Map<ClienteResponseMinDTO>(clienteModel);
    }
    
    /// <summary>
    /// Cria múltiplas entidades do tipo Cliente e as adiciona ao banco de dados.
    /// </summary>
    /// <param name="clienteCollectionDto">Objeto contendo uma coleção de clientes a serem criados.</param>
    /// <returns>Retorna uma lista de DTOs de resposta contendo as informações dos clientes criados.</returns>
    /// <exception cref="KeyNotFoundException">Lançada se o usuário associado aos clientes não for encontrado.</exception>
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

    /// <summary>
    /// Busca uma entidade do tipo Cliente pelo ID.
    /// </summary>
    /// <param name="id">ID do cliente a ser buscado.</param>
    /// <returns>Retorna o DTO de resposta contendo as informações do cliente encontrado.</returns>
    /// <exception cref="ArgumentException">Lançada se o ID for menor ou igual a zero.</exception>
    /// <exception cref="KeyNotFoundException">Lançada se o cliente com o ID especificado não for encontrado.</exception>
    public async Task<ClienteResponseMinDTO?> BuscarPorIdAsync(int id)
    {
        
        if(id <= 0)
            throw new ArgumentException("O ID deve ser maior que zero.", nameof(id));
        
        ClienteModel? cliente = await _clienteRepository.BuscarPorId(id);

        if (cliente == null)
            throw new KeyNotFoundException("Entidade cliente não encontrada");
        
        cliente = RefatoraoMediumClienteModel(cliente);
        
        return _mapper.Map<ClienteResponseMinDTO>(cliente);
    }

    /// <summary>
    /// Busca todas as entidades do tipo Cliente armazenadas no banco de dados.
    /// </summary>
    /// <returns>Retorna uma lista de DTOs de resposta contendo as informações de todos os clientes.</returns>
    public async Task<List<ClienteResponseMinDTO>> BuscarTodosAsync()
    {

        var clienteModels = await _clienteRepository.BuscarTodos();
        
        List<ClienteResponseMinDTO> clientesResponse = clienteModels
            .Select(i => _mapper.Map<ClienteResponseMinDTO>(i))
            .ToList();
        
        return clientesResponse;
    }

    /// <summary>
    /// Remove uma entidade do tipo Cliente pelo ID.
    /// </summary>
    /// <param name="id">ID do cliente a ser removido.</param>
    /// <returns>Retorna true se o cliente for removido com sucesso.</returns>
    /// <exception cref="ArgumentException">Lançada se o ID for menor ou igual a zero.</exception>
    /// <exception cref="KeyNotFoundException">Lançada se o cliente com o ID especificado não for encontrado.</exception>
    public async Task<bool> ApagarAsync(int id)
    {
        
        if(id <= 0)
            throw new ArgumentException("O ID deve ser maior que zero.", nameof(id));
        
        var resultado = await _clienteRepository.Apagar(id);

        if (!resultado)
            throw new KeyNotFoundException("Cliente não encontrado");
        
        return true;
    }

    /// <summary>
    /// Remove todas as entidades do tipo Cliente armazenadas no banco de dados.
    /// </summary>
    /// <returns>Retorna true se todas as entidades forem removidas com sucesso.</returns>
    /// <exception cref="ApplicationException">Lançada se a operação não for realizada com sucesso.</exception>
    public async Task<bool> ApagarTodosAsync()
    {
        var resultado = await _clienteRepository.ApagarTodos();

        if (!resultado)
            throw new ApplicationException("Operação não foi realizada");
        
        return true;

    }

    /// <summary>
    /// Refatora o modelo Cliente para manter apenas as informações necessárias.
    /// </summary>
    /// <param name="clienteModel">Modelo Cliente a ser refatorado.</param>
    /// <returns>Retorna o modelo refatorado de Cliente.</returns>
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
    
    /// <summary>
    /// Refatora o modelo Cliente para manter apenas as informações necessárias para a busca por ID.
    /// </summary>
    /// <param name="clienteModel">Modelo Cliente a ser refatorado.</param>
    /// <returns>Retorna o modelo refatorado de Cliente com informações específicas para a busca por ID.</returns>
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