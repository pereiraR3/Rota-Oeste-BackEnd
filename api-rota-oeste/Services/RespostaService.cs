using api_rota_oeste.Models.Alternativa;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Models.Resposta;
using api_rota_oeste.Models.RespostaAlternativa;
using api_rota_oeste.Models.RespostaTemAlternativa;
using api_rota_oeste.Repositories;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services.Interfaces;
using AutoMapper;

namespace api_rota_oeste.Services;

/**
 * Representa a camada de serviço, isto é, a camada onde fica as regras de negócio da aplicação
 */
public class RespostaService : IRespostaService
{
    
    private readonly IRespostaRepository _respostaRepository;
    private readonly IInteracaoRepository _interacaoRepository;
    private readonly IQuestaoRepository _questaoRepository;
    private readonly IRespostaTemAlternativaRepository _respostaTemAlternativaRepository;
    private readonly IAlternativaRepository _alternativaRepository;
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public RespostaService(
        
        IRespostaRepository respostaRepository,
        IMapper mapper,
        IInteracaoRepository interacaoRepository,
        IQuestaoRepository questaoRepository,
        IRepository repository,
        IRespostaTemAlternativaRepository respostaTemAlternativaRepository,
        IAlternativaRepository alternativaRepository
        
        )
    {
        _respostaRepository = respostaRepository;
        _mapper = mapper;
        _interacaoRepository = interacaoRepository;
        _questaoRepository = questaoRepository;
        _repository = repository;
        _respostaTemAlternativaRepository = respostaTemAlternativaRepository;
        _alternativaRepository = alternativaRepository;
    } 
    
    /**
    * Método da camada de serviço -> para criar uma entidade do tipo RespostaAlternativa
    */
    public async Task<RespostaResponseMinDTO> AdicionarAsync(RespostaRequestDTO respostaRequest)
    {
        if (respostaRequest == null)
            throw new ArgumentNullException(nameof(respostaRequest), "A requisição de resposta não pode ser nula.");

        InteracaoModel? interacaoModel = await _interacaoRepository.BuscarPorId(respostaRequest.InteracaoId);
        if (interacaoModel == null)
            throw new KeyNotFoundException("Interação não encontrada");

        QuestaoModel? questaoModel = await _questaoRepository.BuscarPorId(respostaRequest.QuestaoId);
        if (questaoModel == null)
            throw new KeyNotFoundException("Questão não encontrada");

        if (!AvaliandoRespostaQuestao(questaoModel))
            throw new InvalidOperationException("Operação inválida. Verifique se para a questão respondida há opção de mais de uma alternativa");

        // Criar a nova entidade de resposta
        RespostaModel respostaAlternativaModel = new RespostaModel(respostaRequest, interacaoModel, questaoModel);

        // Adicionar a resposta ao repositório
        RespostaModel resposta = await _respostaRepository.Adicionar(respostaAlternativaModel);

        // Adicionar a resposta às listas de navegação das entidades relacionadas
        interacaoModel.RespostaModels.Add(resposta);
        questaoModel.RespostaModels.Add(resposta);

        // Mapear e retornar o DTO de resposta
        return _mapper.Map<RespostaResponseMinDTO>(resposta);
    }

    /**
    * Método da camada de serviço -> para buscar uma entidade RespostaAlternativa por meio do ID
    */
    public async Task<RespostaResponseMinDTO> BuscarPorIdAsync(int id)
    {
        
        if(id <= 0)
            throw new ArgumentException("O ID deve ser maior que zero.", nameof(id));
        
        RespostaModel? respostaModel = await _respostaRepository.BuscaPorId(id);

        if (respostaModel is null)
            throw new KeyNotFoundException("RespostaAlternativa não encontrada");
        
        return _mapper.Map<RespostaResponseMinDTO>(respostaModel);

    }

    public async Task<RespostaTemAlternativaResponseDTO> AdicionarRespostaTemAlternativaAsync(
        
        int respostaId,
        int alternativaId
        
        )
    {
        
        RespostaModel? respostaModel = await _respostaRepository.BuscaPorId(respostaId);

        if (respostaModel == null)
            throw new KeyNotFoundException("Resposta não encontrada");

        AlternativaModel? alternativaModel = await _alternativaRepository.BuscarPorId(alternativaId);

        if (alternativaModel == null)
            throw new KeyNotFoundException("Alternativa não encontrada");
        
        RespostaTemAlternativaModel respostaTemAlternativaModel = new RespostaTemAlternativaModel(
            respostaId,
            alternativaId,
            respostaModel,
            alternativaModel
        );
        
        // Adicionando a relação de navegabilidade entre as entidades associaas 
        respostaModel.RespostaTemAlternativaModels.Add(respostaTemAlternativaModel);
        alternativaModel.RespostaTemAlternativaModels.Add(respostaTemAlternativaModel);
        
        RespostaTemAlternativaModel? returnRespostaTemAlternativa = await _respostaTemAlternativaRepository.Adicionar(respostaTemAlternativaModel);

        if (returnRespostaTemAlternativa != null)
            returnRespostaTemAlternativa = RefatoracaoMinRespostaTemAlternativaModel(returnRespostaTemAlternativa);
            
        return _mapper.Map<RespostaTemAlternativaResponseDTO>(returnRespostaTemAlternativa);
        
    }

    /**
     * Método da camada de serviço -> para atualizar parcialmente uma entidade RespostaAlternativa
     */
    public async Task<bool> AtualizarAsync(RespostaPatchDTO respostaPatch)
    {
        RespostaModel? respostaModel = await _respostaRepository.BuscaPorId(respostaPatch.Id);

        if (respostaModel == null)
            throw new KeyNotFoundException("RespostaAlternativa não encontrada");
        
        // O mapeamento de atualização deve ignorar campos nulos
        _mapper.Map(respostaPatch, respostaModel);
        
        _repository.Salvar();

        return true;
    }
    
    /**
     * Método da camada de serviço -> para realizar uma deleção relacional de um entidade do tipo RespostaAlternativa
     */
    public async Task<bool> ApagarAsync(int id)
    {
        
        if (id <= 0)
            throw new ArgumentException("O ID deve ser maior que zero.", nameof(id));

        var respostaModel = await _respostaRepository.BuscaPorId(id); 

        if (respostaModel == null)
            throw new KeyNotFoundException("RespostaAlternativa não encontrada.");
        
        await _respostaRepository.Apagar(id);

        return true;
        
    }

    public async Task<bool> ApagarRespostaTemAlternativaAsync(int respostaId, int alternativaId)
    {
        
        var resultado = await _respostaTemAlternativaRepository.Apagar(respostaId, alternativaId);
            
        if(!resultado)
            throw new ApplicationException("Operação não realizada");
            
        return resultado;
        
        
    }

    /**
     * Método da camdda de serviço -> para realizar um deleção relacional em massa de entidades RespostaAlternativa
     */
    public async Task<bool> ApagarTodosAsync()
    {
        var resultado = await _respostaRepository.ApagarTodos();

        if (!resultado)
            throw new ApplicationException("Operação não foi realizada");
        
        return true;
    }

    public RespostaTemAlternativaModel RefatoracaoMinRespostaTemAlternativaModel(
        
        RespostaTemAlternativaModel respostaTemAlternativaModel
        
        )
    {

        respostaTemAlternativaModel.Alternativa = new AlternativaModel
        {
            Id = respostaTemAlternativaModel.Alternativa.Id,
            Descricao = respostaTemAlternativaModel.Alternativa.Descricao,
            Codigo = respostaTemAlternativaModel.Alternativa.Codigo,
            QuestaoId = respostaTemAlternativaModel.Alternativa.QuestaoId
        };

        respostaTemAlternativaModel.Resposta = new RespostaModel
        {
            Id = respostaTemAlternativaModel.Resposta.Id,
            InteracaoId = respostaTemAlternativaModel.Resposta.InteracaoId,
            QuestaoId = respostaTemAlternativaModel.Resposta.QuestaoId
        };

        return respostaTemAlternativaModel;

    }

      /**
     * Método da camdda de serviço -> serve para avaliar se 1 resposta pode ou não estar atrelada a mais de uma
       * alternativa em determinada questão à depender do tipo da Questao
       * Tipos:
       * - QUESTAO_OBJETIVA
       * - QUESTAO_MULTIPLA_ESCOLHA
       * - UPLOAD_DE_IMAGEM
     */
      public bool AvaliandoRespostaQuestao(QuestaoModel questao)
      {
          if (questao == null)
          {
              throw new ArgumentNullException(nameof(questao), "Questão não pode ser nula.");
          }

          int qtda = questao.RespostaModels.Count();

          switch (questao.Tipo)
          {
              case TipoQuestao.QUESTAO_OBJETIVA:
              case TipoQuestao.QUESTAO_UPLOAD_DE_IMAGEM:
                  if (qtda > 0)
                      return false;
                  break;

              default:
                  break;
          }

          return true;
      }

    
}