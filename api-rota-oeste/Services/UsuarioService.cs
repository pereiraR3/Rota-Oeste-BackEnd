using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.Usuario;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services.Interfaces;
using AutoMapper;

namespace api_rota_oeste.Services;

/// <summary>
/// Serviço responsável pelas operações de lógica de negócio relacionadas à entidade Usuario.
/// </summary>
/// <remarks>
/// Implementa a interface <see cref="IUsuarioService"/> e define métodos para adicionar, buscar, atualizar e apagar entidades do tipo Usuario.
/// Este serviço trabalha com as relações entre Usuario, CheckList e Cliente.
/// </remarks>
public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public UsuarioService(
        
        IUsuarioRepository usuarioRepository,
        IMapper mapper,
        IRepository repository
        
        )
    {
        _usuarioRepository = usuarioRepository;
        _mapper = mapper;
        _repository = repository;
    }
    
    /// <summary>
    /// Cria uma nova entidade do tipo Usuario e a adiciona ao banco de dados.
    /// </summary>
    /// <param name="usuarioRequestDto">Objeto contendo os dados do usuário a ser criado.</param>
    /// <returns>Retorna o DTO de resposta contendo as informações do usuário criado.</returns>
    public async Task<UsuarioResponseDTO> AdicionarAsync(UsuarioRequestDTO usuarioRequestDto)
    {
        
        var usuario = new UsuarioModel(usuarioRequestDto);

        UsuarioModel? usuarioModel = await _usuarioRepository.Adicionar(usuario);
        
        return _mapper.Map<UsuarioResponseDTO>(usuarioModel);
    }

    /// <summary>
    /// Busca uma entidade do tipo Usuario pelo ID.
    /// </summary>
    /// <param name="id">ID do usuário a ser buscado.</param>
    /// <returns>Retorna o DTO de resposta contendo as informações do usuário encontrado.</returns>
    /// <exception cref="ArgumentException">Lançada se o ID for menor ou igual a zero.</exception>
    /// <exception cref="KeyNotFoundException">Lançada se o usuário com o ID especificado não for encontrado.</exception>
    public async Task<UsuarioResponseDTO> BuscarPorIdAsync(int id)
    {
        if(id <= 0)
            throw new ArgumentException("O ID deve ser maior que zero.", nameof(id));
        
        UsuarioModel? usuarioModel = await _usuarioRepository.BuscaPorId(id);

        if (usuarioModel == null) 
            throw new KeyNotFoundException("Usuário não encontrado");

        usuarioModel = RefatoraoMinUsuarioModel(usuarioModel);
       
        return _mapper.Map<UsuarioResponseDTO>(usuarioModel);
    }

    /// <summary>
    /// Busca todas as entidades do tipo Usuario armazenadas no banco de dados.
    /// </summary>
    /// <returns>Retorna uma lista de DTOs de resposta contendo as informações de todos os usuários.</returns>
    public async Task<List<UsuarioResponseDTO>> BuscarTodosAsync()
    {
        
        List<UsuarioModel> usuarioModels = await _usuarioRepository.BuscarTodos();
        
        List<UsuarioResponseDTO> usuarioResponseDtos = usuarioModels
            .Select(i => _mapper.Map<UsuarioResponseDTO>(i))
            .ToList();
        
        return usuarioResponseDtos;
    }

    /// <summary>
    /// Atualiza parcialmente uma entidade do tipo Usuario.
    /// </summary>
    /// <param name="usuarioPatchDto">Objeto contendo os dados a serem atualizados no usuário.</param>
    /// <returns>Retorna true se o usuário for atualizado com sucesso, caso contrário retorna false.</returns>
    /// <exception cref="KeyNotFoundException">Lançada se o usuário com o ID especificado não for encontrado.</exception>
    public async Task<bool> AtualizarAsync(UsuarioPatchDTO usuarioPatchDto)
    {
        UsuarioModel? usuarioModel = await _usuarioRepository.BuscaPorId(usuarioPatchDto.Id);
        
        if(usuarioModel == null)
            return false;
        
        // O mapeamento de atualização deve ignorar campos nulos
        _mapper.Map<UsuarioPatchDTO>(usuarioPatchDto);
        
        _repository.Salvar();

        return true;
    }

    /// <summary>
    /// Remove uma entidade do tipo Usuario pelo ID.
    /// </summary>
    /// <param name="id">ID do usuário a ser removido.</param>
    /// <returns>Retorna true se o usuário for removido com sucesso.</returns>
    /// <exception cref="ArgumentException">Lançada se o ID for menor ou igual a zero.</exception>
    /// <exception cref="KeyNotFoundException">Lançada se o usuário com o ID especificado não for encontrado.</exception>
    public async Task<bool> ApagarAsync(int id)
    {
        if(id <= 0)
            throw new ArgumentException("O ID deve ser maior que zero.", nameof(id));
        
        var resultado = await _usuarioRepository.Apagar(id);
        
        if(!resultado)
            throw new KeyNotFoundException("Usuário não encontrado.");
        
        return resultado;
    }

    /// <summary>
    /// Refatora a entidade UsuarioModel para incluir apenas as informações julgadas como necessárias.
    /// </summary>
    /// <param name="usuarioModel">Modelo de usuário que será refatorado.</param>
    /// <returns>Retorna o modelo de usuário refatorado.</returns>
    public UsuarioModel RefatoraoMinUsuarioModel(UsuarioModel usuarioModel)
    {
        var checkListModelsRefatorado = usuarioModel.CheckLists
            .Select(o => new CheckListModel
            {
                Id = o.Id,
                UsuarioId = o.UsuarioId,
                Nome = o.Nome,
                DataCriacao = o.DataCriacao
            }).ToList();

        var clientModelsRefatorado = usuarioModel.Clientes
            .Select(o => new ClienteModel
            {
                Id = o.Id,
                UsuarioId = o.UsuarioId,
                Nome = o.Nome,
                Telefone = o.Telefone
            }).ToList();

        usuarioModel.CheckLists = checkListModelsRefatorado;
        usuarioModel.Clientes = clientModelsRefatorado;

        return usuarioModel;
    }
    
}