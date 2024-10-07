﻿using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Models.RespostaAlternativa;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services.Interfaces;
using AutoMapper;

namespace api_rota_oeste.Services;

/// <summary>
/// Serviço responsável pelas operações de lógica de negócio relacionadas à entidade Interacao.
/// </summary>
/// <remarks>
/// Implementa a interface <see cref="IInteracaoService"/> e define métodos para adicionar, buscar, atualizar e apagar entidades do tipo Interacao.
/// Também gerencia o relacionamento entre as entidades Cliente, CheckList e RespostaAlternativa.
/// </remarks>
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
    
    /// <summary>
    /// Cria uma nova entidade do tipo Interacao e a adiciona ao banco de dados.
    /// </summary>
    /// <param name="interacaoDto">Objeto contendo os dados da interação a ser criada.</param>
    /// <returns>Retorna o DTO de resposta contendo as informações da interação criada.</returns>
    /// <exception cref="KeyNotFoundException">Lançada se o cliente ou o checklist associado à interação não for encontrado.</exception>
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
    
    /// <summary>
    /// Busca uma entidade do tipo Interacao pelo ID.
    /// </summary>
    /// <param name="id">ID da interação a ser buscada.</param>
    /// <returns>Retorna o DTO de resposta contendo as informações da interação encontrada.</returns>
    /// <exception cref="ArgumentException">Lançada se o ID for menor ou igual a zero.</exception>
    /// <exception cref="ArgumentNullException">Lançada se a interação com o ID especificado não for encontrada.</exception>
    public async Task<InteracaoResponseDTO> BuscarPorIdAsync(int id)
    {
        if(id <= 0)
            throw new ArgumentException("O ID deve ser maior que zero.", nameof(id));
        
        var interacao = await _repositoryInteracao.BuscarPorId(id);
        
        if(interacao == null) throw new ArgumentNullException(nameof(id));

        interacao = RefatoraoMediumInteracaoModel(interacao);

        return _mapper.Map<InteracaoResponseDTO>(interacao);
    }

    /// <summary>
    /// Busca todas as entidades do tipo Interacao armazenadas no banco de dados.
    /// </summary>
    /// <returns>Retorna uma lista de DTOs de resposta contendo as informações de todas as interações.</returns>
    public async Task<List<InteracaoResponseDTO>> BuscarTodosAsync()
    {
        var interacao = await _repositoryInteracao.BuscarTodos();
        
        return interacao
            .Select(i => _mapper.Map<InteracaoResponseDTO>(RefatoraoMinInteracaoModel(i)))
            .ToList();
    }

    /// <summary>
    /// Atualiza parcialmente uma entidade do tipo Interacao.
    /// </summary>
    /// <param name="interacaoPatch">Objeto contendo os dados a serem atualizados na interação.</param>
    /// <returns>Retorna true se a interação for atualizada com sucesso.</returns>
    /// <exception cref="KeyNotFoundException">Lançada se a interação com o ID especificado não for encontrada.</exception>
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
    
    /// <summary>
    /// Remove uma entidade do tipo Interacao pelo ID.
    /// </summary>
    /// <param name="id">ID da interação a ser removida.</param>
    /// <returns>Retorna true se a interação for removida com sucesso.</returns>
    /// <exception cref="ArgumentException">Lançada se o ID for menor ou igual a zero.</exception>
    /// <exception cref="KeyNotFoundException">Lançada se a interação com o ID especificado não for encontrada.</exception>
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

    /// <summary>
    /// Remove todas as entidades do tipo Interacao armazenadas no banco de dados.
    /// </summary>
    /// <returns>Retorna true se todas as interações forem removidas com sucesso.</returns>
    public async Task<bool> ApagarTodosAsync()
    {

        await _repositoryInteracao.ApagarTodos();

        return true;

    }
    
    /// <summary>
    /// Refatora o modelo Interacao para manter apenas as informações necessárias para a busca por ID.
    /// </summary>
    /// <param name="interacaoModel">Modelo Interacao a ser refatorado.</param>
    /// <returns>Retorna o modelo refatorado de Interacao.</returns>
    public InteracaoModel RefatoraoMediumInteracaoModel(InteracaoModel interacaoModel)
    {
        var interacaoRespostaAlternativaModels = interacaoModel.RespostaAlternativaModels
            .Select(o => new RespostaModel
            {
                Id = o.Id,
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

    /// <summary>
    /// Refatora o modelo Interacao para manter apenas as informações necessárias para a criação da entidade.
    /// </summary>
    /// <param name="interacaoModel">Modelo Interacao a ser refatorado.</param>
    /// <returns>Retorna o modelo refatorado de Interacao.</returns>
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