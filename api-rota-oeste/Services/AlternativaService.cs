using api_rota_oeste.Models.Alternativa;
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Repositories;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services.Interfaces;
using AutoMapper;

namespace api_rota_oeste.Services;

public class AlternativaService : IAlternativaService
{
    
    private readonly IAlternativaRepository _repositoryAlternativa;
    private readonly IQuestaoRepository _repositoryQuestao;
    private readonly IRepository _repository;
    private readonly IMapper _mapper;
    
    public AlternativaService(
        
        IAlternativaRepository repository,
        IMapper mapper,
        IQuestaoRepository repositoryQuestao,
        IRepository repository1
        
        )
    {
        _mapper = mapper;
        _repositoryQuestao = repositoryQuestao;
        _repository = repository1;
        _repositoryAlternativa = repository;
    }
    
    /**
    * Método da camada de serviço -> para criar uma entidade do tipo alternativa
   */
    public async Task<AlternativaResponseDTO> AdicionarAsync(AlternativaRequestDTO alternativaRequest)
    {

        QuestaoModel? questaoModel = await _repositoryQuestao.BuscarPorId(alternativaRequest.QuestaoId);

        if (questaoModel == null)
            throw new KeyNotFoundException("Questão não encontrado");
        
        // Obter o próximo valor do Código para a questão
        int proximoCodigo = await _repositoryAlternativa.ContarPorQuestaoId(alternativaRequest.QuestaoId) + 1;
        
        // Criar a nova alternativa com o próximo código
        var alternativaModel = new AlternativaModel(alternativaRequest, questaoModel, proximoCodigo);

        // Adicionar a alternativa ao repositório
        var alternativa = await _repositoryAlternativa.Adicionar(alternativaModel);

        // Adicionar a alternativa à lista de alternativas da questão
        questaoModel.AlternativaModels.Add(alternativa);

        // Mapear para o DTO de resposta e retornar
        return _mapper.Map<AlternativaResponseDTO>(alternativa);
        
    }
    
    /**
    * Método da camada de serviço -> para buscar uma entidade do tipo alternativa pelo ID
    */
    public async Task<AlternativaResponseDTO> BuscarPorIdAsync(int id)
    {
        if(id <= 0)
            throw new ArgumentException("O ID deve ser maior que zero.", nameof(id));
        
        var alternativa = await _repositoryAlternativa.BuscarPorId(id);
        
        if (alternativa == null)
        {
            throw new KeyNotFoundException("Não há alternativa registrada com o ID informado.");
        }

        alternativa.Questao = new QuestaoModel
        {
            Id = alternativa.QuestaoId,
            Titulo = alternativa.Questao.Titulo,
            Tipo = alternativa.Questao.Tipo
        };
        
        return _mapper.Map<AlternativaResponseDTO>(alternativa);
        
    }
    
    
    /**
    * Método da camada de serviço -> para atualizar um entidade do tipo alternativa
    */
    public async Task<List<AlternativaResponseDTO>> BuscarTodosAsync()
    {
        var alternativas = await _repositoryAlternativa.BuscarTodos();
        
        return alternativas
            .Select(_mapper.Map<AlternativaResponseDTO>)
            .ToList();
    }
    
    /**
    * Método da camada de serviço -> para fazer a atualização de um entidade do tipo alternativa
    */
    public async Task<bool> AtualizarAsync(AlternativaPatchDTO alternativaPatch)
    {
        AlternativaModel? alternativaModel = await _repositoryAlternativa.BuscarPorId(alternativaPatch.Id);

        if (alternativaModel == null)
            throw new KeyNotFoundException("Alternativa não encontrada");
        
        // O mapeamento de atualização deve ignorar campos nulos
        _mapper.Map(alternativaPatch, alternativaModel);
        
        _repository.Salvar();

        return true;
    }
    
    /**
    * Método da camada de serviço -> para fazer a deleção relacional de uma entidade do tipo alternativa
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