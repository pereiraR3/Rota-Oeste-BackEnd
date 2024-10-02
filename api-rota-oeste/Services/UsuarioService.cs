using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.Usuario;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services.Interfaces;
using AutoMapper;

namespace api_rota_oeste.Services;

/**
 * Representa a camada de serviço, isto é, a camada onde fica as regras de negócio da aplicação
 */
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
    
    /**
     * Método da camada de serviço -> para criar uma entidade do tipo usuário
     */
    public async Task<UsuarioResponseDTO> AdicionarAsync(UsuarioRequestDTO usuarioRequestDto)
    {
        
        var usuario = new UsuarioModel(usuarioRequestDto);

        UsuarioModel? usuarioModel = await _usuarioRepository.Adicionar(usuario);
        
        return _mapper.Map<UsuarioResponseDTO>(usuarioModel);
    }

    /**
     * Método da camada de serviço -> para buscar uma entidade do tipo usuário pelo ID
     */
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

    /**
    * Método da camada de serviço -> para buscar todas as entidades do tipo usuario
    */
    public async Task<List<UsuarioResponseDTO>> BuscarTodosAsync()
    {
        
        List<UsuarioModel> usuarioModels = await _usuarioRepository.BuscarTodos();
        
        List<UsuarioResponseDTO> usuarioResponseDtos = usuarioModels
            .Select(i => _mapper.Map<UsuarioResponseDTO>(i))
            .ToList();
        
        return usuarioResponseDtos;
    }

    /**
     * Método da camada de serviço -> para atualizar um entidade do tipo usuário 
     */
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

    /**
     * Método da camada de serviço -> para fazer a deleção relacional de uma entidade do tipo usuário
     */
    public async Task<bool> ApagarAsync(int id)
    {
        if(id <= 0)
            throw new ArgumentException("O ID deve ser maior que zero.", nameof(id));
        
        var resultado = await _usuarioRepository.Apagar(id);
        
        if(!resultado)
            throw new KeyNotFoundException("Usuário não encontrado.");
        
        return resultado;
    }

    /**
     * Método da camada de serviço -> para fazer a refatoracao dos DTOs, de modo que puxem apenas as
     * informações que foram julgadas como necessárias
     */
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