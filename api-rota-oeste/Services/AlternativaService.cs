using api_rota_oeste.Models.Alternativa;
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Repositories;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services.Interfaces;
using AutoMapper;

namespace api_rota_oeste.Services;

/// <summary>
/// Serviço responsável pelas operações de lógica de negócio relacionadas à entidade Alternativa.
/// </summary>
/// <remarks>
/// Implementa a interface <see cref="IAlternativaService"/> e define métodos para adicionar, buscar, atualizar e apagar entidades do tipo Alternativa.
/// Além disso, realiza validações e mapeamentos necessários para operações relacionadas à entidade Alternativa.
/// </remarks>
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
    
    /// <summary>
    /// Cria uma nova entidade do tipo Alternativa e a adiciona ao banco de dados.
    /// </summary>
    /// <param name="alternativaRequest">Objeto contendo os dados da alternativa a ser criada.</param>
    /// <returns>Retorna o DTO de resposta contendo as informações da alternativa criada.</returns>
    /// <exception cref="KeyNotFoundException">Lançada se a questão associada à alternativa não for encontrada.</exception>
    public async Task<AlternativaResponseMinDTO> AdicionarAsync(AlternativaRequestDTO alternativaRequest)
    {

        QuestaoModel? questaoModel = await _repositoryQuestao.BuscarPorId(alternativaRequest.QuestaoId);

        if (questaoModel == null)
            throw new KeyNotFoundException("Questão não encontrado");
        
        // Obter o próximo valor do Código para a questão
        int proximoCodigo = await _repositoryAlternativa.ObterProximoCodigoPorQuestaoId(alternativaRequest.QuestaoId);
        
        // Criar a nova alternativa com o próximo código
        var alternativaModel = new AlternativaModel(alternativaRequest, questaoModel, proximoCodigo);

        // Adicionar a alternativa ao repositório
        var alternativa = await _repositoryAlternativa.Adicionar(alternativaModel);

        // Adicionar a alternativa à lista de alternativas da questão
        questaoModel.AlternativaModels.Add(alternativa);
        
        // Mapear para o DTO de resposta e retornar
        return _mapper.Map<AlternativaResponseMinDTO>(alternativa);
        
    }
    
    /// <summary>
    /// Busca uma entidade do tipo Alternativa pelo ID.
    /// </summary>
    /// <param name="id">ID da alternativa a ser buscada.</param>
    /// <returns>Retorna o DTO de resposta contendo as informações da alternativa encontrada.</returns>
    /// <exception cref="ArgumentException">Lançada se o ID for menor ou igual a zero.</exception>
    /// <exception cref="KeyNotFoundException">Lançada se a alternativa com o ID especificado não for encontrada.</exception>
    public async Task<AlternativaResponseMinDTO> BuscarPorIdAsync(int id)
    {
        if(id <= 0)
            throw new ArgumentException("O ID deve ser maior que zero.", nameof(id));
        
        var alternativa = await _repositoryAlternativa.BuscarPorId(id);
        
        if (alternativa == null)
        {
            throw new KeyNotFoundException("Não há alternativa registrada com o ID informado.");
        }

        // Refatorando AlternativaModel
        alternativa = RefatoracaoMinAlternativaModel(alternativa);
        
        return _mapper.Map<AlternativaResponseMinDTO>(alternativa);
        
    }
    
    /// <summary>
    /// Busca todas as entidades do tipo Alternativa armazenadas no banco de dados.
    /// </summary>
    /// <returns>Retorna uma lista de DTOs de resposta contendo as informações de todas as alternativas.</returns>
    public async Task<List<AlternativaResponseMinDTO>> BuscarTodosAsync()
    {
        List<AlternativaModel> alternativas = await _repositoryAlternativa.BuscarTodos();
        
        return alternativas
            .Select(RefatoracaoMinAlternativaModel)
            .Select(refatorado => _mapper.Map<AlternativaResponseMinDTO>(refatorado))
            .ToList();

    }
    
    /// <summary>
    /// Atualiza os dados de uma entidade do tipo Alternativa com base no DTO fornecido.
    /// </summary>
    /// <param name="alternativaPatch">Objeto contendo os dados a serem atualizados da alternativa.</param>
    /// <returns>Retorna true se a atualização for bem-sucedida, caso contrário, retorna false.</returns>
    /// <exception cref="KeyNotFoundException">Lançada se a alternativa com o ID especificado não for encontrada.</exception>
    public async Task<bool> AtualizarAsync(AlternativaPatchDTO alternativaPatch)
    {
        AlternativaModel? alternativaModel = await _repositoryAlternativa.BuscarPorId(alternativaPatch.Id);

        if (alternativaModel == null)
            throw new KeyNotFoundException("Alternativa não encontrada");

        alternativaModel.Descricao = alternativaPatch.Descricao ?? alternativaModel.Descricao;

        _repository.Salvar();

        return true;
    }
    
    /// <summary>
    /// Remove uma entidade do tipo Alternativa do banco de dados pelo ID.
    /// </summary>
    /// <param name="id">ID da alternativa a ser removida.</param>
    /// <returns>Retorna true se a alternativa for removida com sucesso, caso contrário, lança uma exceção.</returns>
    /// <exception cref="ArgumentException">Lançada se o ID for menor ou igual a zero.</exception>
    /// <exception cref="KeyNotFoundException">Lançada se a questão associada não for encontrada.</exception>
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

    /// <summary>
    /// Refatora a entidade Alternativa para garantir que apenas as informações necessárias sejam mantidas.
    /// </summary>
    /// <param name="alternativaModel">Modelo de alternativa a ser refatorado.</param>
    /// <returns>Retorna o modelo de alternativa refatorado.</returns>
    /// <exception cref="ArgumentNullException">Lançada se o modelo de alternativa for nulo.</exception>
    public AlternativaModel RefatoracaoMinAlternativaModel(AlternativaModel alternativaModel)
    {
        if (alternativaModel == null)
        {
            throw new ArgumentNullException(nameof(alternativaModel), "O modelo de alternativa não pode ser nulo.");
        }

        if (alternativaModel.Questao == null)
        {
            alternativaModel.Questao = new QuestaoModel
            {
                Id = alternativaModel.QuestaoId,
                Titulo = "Título Padrão", // Alterado para não causar NullReferenceException
                Tipo = TipoQuestao.QUESTAO_OBJETIVA // Alterado para não causar NullReferenceException
            };
        }

        return alternativaModel;
    }
    
}