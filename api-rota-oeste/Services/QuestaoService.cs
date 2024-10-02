using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Repositories;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services.Interfaces;
using AutoMapper;

namespace api_rota_oeste.Services;

/**
 * Representa a camada de serviço, isto é, a camada onde fica as regras de negócio da aplicação
 */
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
    
    /**
    * Método da camada de serviço -> para criar uma entidade do tipo questao
   */
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
    
    /**
    * Método da camada de serviço -> para buscar uma entidade do tipo questao pelo ID
    */
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
    
    
    /**
    * Método da camada de serviço -> para atualizar um entidade do tipo questao
    */
    public async Task<List<QuestaoResponseDTO>> BuscarTodosAsync()
    {
        var questoes = await _repositoryQuestao.BuscarTodos();
        
        return questoes.Select(_mapper.Map<QuestaoResponseDTO>).ToList();
    }
    
    /**
    * Método da camada de serviço -> para fazer a atualização de um entidade do tipo questao
    */
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
    
    
    /**
    * Método da camada de serviço -> para fazer a deleção relacional de uma entidade do tipo questao
    */
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