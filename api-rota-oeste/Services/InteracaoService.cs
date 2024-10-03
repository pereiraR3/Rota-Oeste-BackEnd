﻿using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Models.RespostaAlternativa;
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

        if (clienteModel == null)
            throw new KeyNotFoundException("Cliente não encontrado");
        
        CheckListModel? checkListModel = await _checkListRepository.BuscarPorId(interacaoDto.CheckListId);

        if (checkListModel == null)
            throw new KeyNotFoundException("CheckList não encontrado");
        
        InteracaoModel interacaoModel = new InteracaoModel(interacaoDto, clienteModel, checkListModel);
        
        InteracaoModel? interacao = await _repositoryInteracao.Adicionar(interacaoModel);

        if(interacao != null)
            interacao = RefatoraoMinInteracaoModel(interacao);
        
        return _mapper.Map<InteracaoResponseDTO>(interacao);
        
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

        interacao = RefatoraoMediumInteracaoModel(interacao);

        return _mapper.Map<InteracaoResponseDTO>(interacao);
    }

    /**
    * Método da camada de servico -> para buscar todas as entidades do tipo interacao
    */
    public async Task<List<InteracaoResponseDTO>> BuscarTodosAsync()
    {
        var interacao = await _repositoryInteracao.BuscarTodos();
        
        return interacao
            .Select(i => _mapper.Map<InteracaoResponseDTO>(RefatoraoMinInteracaoModel(i)))
            .ToList();
    }

    /**
     * Método da camada de servico -> para atualizar parcialmente uma entidade interacao
     */
    public async Task<bool> AtualizarAsync(InteracaoPatchDTO interacaoPatch)
    {
        
        var interacaoModel = await _repositoryInteracao.BuscarPorId(interacaoPatch.Id);
        
        if(interacaoModel == null)
            throw new KeyNotFoundException("Interação não encontrada");
        
        // O mapeamento de atualização deve ignorar campos nulos
        _mapper.Map(interacaoPatch, interacaoModel);
            
        _repository.Salvar();

        return true;
    }
    
    /**
     * Método da camada de servico -> para apagar uma determinada interacao
     */
    public async Task<bool> ApagarAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("O ID deve ser maior que zero.", nameof(id));

        var interacao = await _repositoryInteracao.BuscarPorId(id); 

        if (interacao == null)
            throw new KeyNotFoundException("Questão não encontrada.");
        
        await _repositoryInteracao.ApagarPorId(id);

        return true;
    }

    /**
     * Método da camada de servico -> para apagar todas as entidades interacao
     */
    public async Task<bool> ApagarTodosAsync()
    {

        await _repositoryInteracao.ApagarTodos();

        return true;

    }
    
    /**
     * Método da camada de serviço -> para fazer a refatoracao de InteracaoModel -> InteracaoResponseDTO sem que haja
     * problemas de dependência circular de modo que puxem apenas as informações que foram julgadas como necessárias
     * ao método GET 
     */
    public InteracaoModel RefatoraoMediumInteracaoModel(InteracaoModel interacaoModel)
    {
        var interacaoRespostaAlternativaModels = interacaoModel.RespostaAlternativaModels
            .Select(o => new RespostaModel
            {
                Id = o.Id,
                Alternativa = o.Alternativa,
                Questao = o.Questao,
                Interacao = o.Interacao
                
            }).ToList();
        
        interacaoModel.RespostaAlternativaModels = interacaoRespostaAlternativaModels;

        if (interacaoModel.CheckList != null)
            interacaoModel.CheckList = new CheckListModel
            {
                Id = interacaoModel.CheckList.Id,
                Usuario = interacaoModel.CheckList.Usuario,
                Nome = interacaoModel.CheckList.Nome,
                DataCriacao = interacaoModel.CheckList.DataCriacao
            };

        if (interacaoModel.Cliente != null)
            interacaoModel.Cliente = new ClienteModel
            {
                Id = interacaoModel.Cliente.Id,
                UsuarioId = interacaoModel.Cliente.UsuarioId,
                Nome = interacaoModel.Cliente.Nome,
                Telefone = interacaoModel.Cliente.Telefone,
            };
        
        return interacaoModel;
    }

    /**
     * Método da camada de serviço -> para fazer a refatoracao de InteracaoModel -> InteracaoResponseDTO sem que haja
     * problemas de dependência circular de modo que puxem apenas as informações que foram julgadas como necessárias
     * para retornar ao método POST de criação
     */
    public InteracaoModel RefatoraoMinInteracaoModel(InteracaoModel interacaoModel)
    {
        interacaoModel.RespostaAlternativaModels = new List<RespostaModel>();

        if (interacaoModel.CheckList != null)
            interacaoModel.CheckList = new CheckListModel
            {
                Id = interacaoModel.CheckList.Id,
                Usuario = interacaoModel.CheckList.Usuario,
                Nome = interacaoModel.CheckList.Nome,
                DataCriacao = interacaoModel.CheckList.DataCriacao
            };

        if (interacaoModel.Cliente != null)
            interacaoModel.Cliente = new ClienteModel
            {
                Id = interacaoModel.Cliente.Id,
                UsuarioId = interacaoModel.Cliente.UsuarioId,
                Nome = interacaoModel.Cliente.Nome,
                Telefone = interacaoModel.Cliente.Telefone,
            };
        
        
        return interacaoModel;
    }
    
}