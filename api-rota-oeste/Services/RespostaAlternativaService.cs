using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Models.RespostaAlternativa;
using api_rota_oeste.Repositories;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services.Interfaces;
using AutoMapper;

namespace api_rota_oeste.Services;

/**
 * Representa a camada de serviço, isto é, a camada onde fica as regras de negócio da aplicação
 */
public class RespostaAlternativaService : IRespostaAlternativaService
{
    
    private readonly IRespostaAlternativaRepository _respostaAlternativaRepository;
    private readonly IInteracaoRepository _interacaoRepository;
    private readonly IQuestaoRepository _questaoRepository;
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public RespostaAlternativaService(
        
        IRespostaAlternativaRepository respostaAlternativaRepository,
        IMapper mapper,
        IInteracaoRepository interacaoRepository,
        IQuestaoRepository questaoRepository, IRepository repository)
    {
        _respostaAlternativaRepository = respostaAlternativaRepository;
        _mapper = mapper;
        _interacaoRepository = interacaoRepository;
        _questaoRepository = questaoRepository;
        _repository = repository;
    } 
    
    /**
    * Método da camada de serviço -> para criar uma entidade do tipo RespostaAlternativa
    */
    public async Task<RespostaAlternativaResponseDTO> AdicionarAsync(RespostaAlternativaRequestDTO respostaAlternativa)
    {

        InteracaoModel? interacaoModel = await _interacaoRepository.BuscarPorId(respostaAlternativa.InteracaoId);
        
        QuestaoModel? questaoModel = await _questaoRepository.BuscarPorId(respostaAlternativa.QuestaoId);
        
        if(interacaoModel == null)
            throw new KeyNotFoundException("Interação não encontrada");
        
        if(questaoModel == null)
            throw new KeyNotFoundException("Questão não encontrada");
        
        RespostaAlternativaModel respostaAlternativaModel = new RespostaAlternativaModel(respostaAlternativa, interacaoModel, questaoModel);
        
        RespostaAlternativaModel? resposta = await _respostaAlternativaRepository.Adicionar(respostaAlternativaModel);

        if (resposta != null)
        {
            interacaoModel.RespostaAlternativaModels.Add(resposta);
            questaoModel.RespostaAlternativaModels.Add(resposta);

        }
        
        return _mapper.Map<RespostaAlternativaResponseDTO>(respostaAlternativaModel);
        
    }

    /**
    * Método da camada de serviço -> para buscar uma entidade RespostaAlternativa por meio do ID
    */
    public async Task<RespostaAlternativaResponseDTO> BuscarPorIdAsync(int id)
    {
        
        if(id <= 0)
            throw new ArgumentException("O ID deve ser maior que zero.", nameof(id));
        
        RespostaAlternativaModel? respostaAlternativaModel = await _respostaAlternativaRepository.BuscaPorId(id);

        if (respostaAlternativaModel is null)
            throw new KeyNotFoundException("RespostaAlternativa não encontrada");
        
        return _mapper.Map<RespostaAlternativaResponseDTO>(respostaAlternativaModel);

    }

    /**
     * Método da camada de serviço -> para atualizar parcialmente uma entidade RespostaAlternativa
     */
    public async Task<bool> AtualizarAsync(RespostaAlternativaPatchDTO respostaAlternativaPatch)
    {
        RespostaAlternativaModel? respostaAlternativaModel = await _respostaAlternativaRepository.BuscaPorId(respostaAlternativaPatch.Id);

        if (respostaAlternativaModel == null)
            throw new KeyNotFoundException("RespostaAlternativa não encontrada");
        
        // O mapeamento de atualização deve ignorar campos nulos
        _mapper.Map(respostaAlternativaPatch, respostaAlternativaModel);
        
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

        var respostaModel = await _respostaAlternativaRepository.BuscaPorId(id); 

        if (respostaModel == null)
            throw new KeyNotFoundException("RespostaAlternativa não encontrada.");
        
        await _respostaAlternativaRepository.Apagar(id);

        return true;
        
    }

    /**
     * Método da camdda de serviço -> para realizar um deleção relacional em massa de entidades RespostaAlternativa
     */
    public async Task<bool> ApagarTodosAsync()
    {
        var resultado = await _respostaAlternativaRepository.ApagarTodos();

        if (!resultado)
            throw new ApplicationException("Operação não foi realizada");
        
        return true;
    }
    
}