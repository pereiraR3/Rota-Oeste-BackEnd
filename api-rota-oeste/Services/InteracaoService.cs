using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services.Interfaces;
using AutoMapper;

namespace api_rota_oeste.Services;

/**
 * Representa a camada de serviço, isto é, a camada onde fica as regras de negócio da aplicação
 */
public class InteracaoService : IInteracaoService {
    
    private readonly IInteracaoRepository _repositoryInteracao;
    private readonly IClienteRepository _clienteRepository;
    private readonly ICheckListRepository _checkListRepository;
    private readonly IRepository  _repository;
    private readonly IMapper _mapper;
    
    public InteracaoService(
        
        IInteracaoRepository repository,
        IMapper mapper,
        IClienteRepository clienteRepository,
        ICheckListRepository checkListRepository,
        IRepository repository1
        
        )
    {
        _mapper = mapper;
        _clienteRepository = clienteRepository;
        _checkListRepository = checkListRepository;
        _repository = repository1;
        _repositoryInteracao = repository;
    }
    
    /**
     * Método da camada de servico -> para criar uma entidade do tipo interacao
     */
    public async Task<InteracaoResponseDTO> AdicionarAsync(InteracaoRequestDTO interacaoDto) {
        
        ClienteModel? clienteModel = await _clienteRepository.BuscarPorId(interacaoDto.ClienteId);
        
        CheckListModel? checkListModel = await _checkListRepository.BuscarPorId(interacaoDto.CheckListId);
        
        if(checkListModel == null || clienteModel == null)
            return null;
        
        InteracaoModel? interacaoModel = new InteracaoModel(interacaoDto, clienteModel, checkListModel);
        
        var resultado = await _repositoryInteracao.Adicionar(interacaoModel);
        
        return _mapper.Map<InteracaoResponseDTO>(interacaoModel);
        
    }
    
    /**
     * Método da camada de servico -> para buscar uma entidade interacao por ID
     */
    public async Task<InteracaoResponseDTO> BuscarPorIdAsync(int id)
    {
        if(id <= 0)
            throw new ArgumentException("O ID deve ser maior que zero.", nameof(id));
        
        var interacao = await _repositoryInteracao.BuscarPorId(id);
        
        if(interacao == null) throw new ArgumentNullException(nameof(id));

        return _mapper.Map<InteracaoResponseDTO>(interacao);
    }

    /**
     * Método da camada de servico -> para atualizar parcialmente uma entidade interacao
     */
    public Task<bool> AtualizarAsync(InteracaoPatchDTO req)
    {

        try
        {
            _mapper.Map<InteracaoPatchDTO>(req);
            
            _repository.Salvar();

            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
    
}