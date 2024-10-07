using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Repositories;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services.Interfaces;
using AutoMapper;

namespace api_rota_oeste.Services;

/// <summary>
/// Serviço responsável pelas operações de lógica de negócio relacionadas à entidade Questao.
/// </summary>
/// <remarks>
/// Implementa a interface <see cref="IQuestaoService"/> e define métodos para adicionar, buscar, atualizar e apagar entidades do tipo Questao.
/// Este serviço trabalha com as relações entre Questao e CheckList.
/// </remarks>
public class QuestaoService : IQuestaoService{
    
    private readonly IQuestaoRepository _repositoryQuestao;
    private readonly ICheckListRepository _repositoryCheckList;
    private readonly IRepository _repository;
    private readonly IMapper _mapper;
    
    public QuestaoService(
        
        IQuestaoRepository repository,
        IMapper mapper,
        ICheckListRepository repositoryCheckList,
        IRepository repository1
        
        )
    {
        _mapper = mapper;
        _repositoryCheckList = repositoryCheckList;
        _repository = repository1;
        _repositoryQuestao = repository;
    }
    
    /// <summary>
    /// Cria uma nova entidade do tipo Questao e a adiciona ao banco de dados.
    /// </summary>
    /// <param name="questaoRequest">Objeto contendo os dados da questão a ser criada.</param>
    /// <returns>Retorna o DTO de resposta contendo as informações da questão criada.</returns>
    /// <exception cref="KeyNotFoundException">Lançada se o checklist associado à questão não for encontrado.</exception>
    public async Task<QuestaoResponseDTO> AdicionarAsync(QuestaoRequestDTO questaoRequest)
    {

        CheckListModel? checkListModel = await _repositoryCheckList.BuscarPorId(questaoRequest.CheckListId);

        if (checkListModel == null)
            throw new KeyNotFoundException("CheckList não encontrado");
        
        var questaoModel = new QuestaoModel(questaoRequest, checkListModel);
        
        var questao = await _repositoryQuestao.Adicionar(questaoModel);
        
        checkListModel.Questoes.Add(questao);
        
        return _mapper.Map<QuestaoResponseDTO>(questao);
        
    }
    
    /// <summary>
    /// Busca uma entidade do tipo Questao pelo ID.
    /// </summary>
    /// <param name="id">ID da questão a ser buscada.</param>
    /// <returns>Retorna o DTO de resposta contendo as informações da questão encontrada.</returns>
    /// <exception cref="ArgumentException">Lançada se o ID for menor ou igual a zero.</exception>
    /// <exception cref="KeyNotFoundException">Lançada se a questão com o ID especificado não for encontrada.</exception>
    public async Task<QuestaoResponseDTO> BuscarPorIdAsync(int id)
    {
        if(id <= 0)
            throw new ArgumentException("O ID deve ser maior que zero.", nameof(id));
        
        var questao = await _repositoryQuestao.BuscarPorId(id);
        
        if (questao == null)
        {
            throw new KeyNotFoundException("Não há questão registrada com o ID informado.");
        }
        
        var questaoConvertida = _mapper.Map<QuestaoResponseDTO>(questao);
        
        return questaoConvertida;
    }
    
    
    /// <summary>
    /// Busca todas as entidades do tipo Questao armazenadas no banco de dados.
    /// </summary>
    /// <returns>Retorna uma lista de DTOs de resposta contendo as informações de todas as questões.</returns>
    public async Task<List<QuestaoResponseDTO>> BuscarTodosAsync()
    {
        var questoes = await _repositoryQuestao.BuscarTodos();
        
        return questoes
            .Select(_mapper.Map<QuestaoResponseDTO>)
            .ToList();
    }
    
    /// <summary>
    /// Atualiza parcialmente uma entidade do tipo Questao.
    /// </summary>
    /// <param name="questaoPatch">Objeto contendo os dados a serem atualizados na questão.</param>
    /// <returns>Retorna true se a questão for atualizada com sucesso.</returns>
    /// <exception cref="KeyNotFoundException">Lançada se a questão com o ID especificado não for encontrada.</exception>
    public async Task<bool> AtualizarAsync(QuestaoPatchDTO questaoPatch)
    {
        QuestaoModel? questaoModel = await _repositoryQuestao.BuscarPorId(questaoPatch.Id);

        if (questaoModel == null)
            throw new KeyNotFoundException("Questão não encontrada");
        
        // O mapeamento de atualização deve ignorar campos nulos
        _mapper.Map(questaoPatch, questaoModel);
        
        _repository.Salvar();

        return true;
    }
    
    /// <summary>
    /// Remove uma entidade do tipo Questao pelo ID.
    /// </summary>
    /// <param name="id">ID da questão a ser removida.</param>
    /// <returns>Retorna true se a questão for removida com sucesso.</returns>
    /// <exception cref="ArgumentException">Lançada se o ID for menor ou igual a zero.</exception>
    /// <exception cref="KeyNotFoundException">Lançada se a questão com o ID especificado não for encontrada.</exception>
    public async Task<bool> ApagarAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("O ID deve ser maior que zero.", nameof(id));

        var questaoObtida = await _repositoryQuestao.BuscarPorId(id); 

        if (questaoObtida == null)
        {
            throw new KeyNotFoundException("Questão não encontrada.");
        }

        await _repositoryQuestao.Apagar(id);

        return true;
    }
    
}