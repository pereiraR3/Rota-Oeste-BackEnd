using api_rota_oeste.Models.Alternativa;
using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Models.WppMessage;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Models.RespostaAlternativa;

namespace api_rota_oeste.Services.Utils;

public class MessageOrchestrationService
{
    private readonly ICheckListProcessService _checkListProcessService;
    private readonly IWhatsAppService _whatsAppService;
    private readonly IClienteRepository _clienteRepository;
    private readonly ILogger<MessageOrchestrationService> _logger;
    private readonly IInteracaoRepository _interacaoRepository;
    private readonly IRespostaService _respostaService;

    public MessageOrchestrationService(
        ICheckListProcessService checkListProcessService, 
        IWhatsAppService whatsAppService, 
        IClienteRepository clienteRepository, 
        ILogger<MessageOrchestrationService> logger, 
        IInteracaoRepository interacaoRepository,
        IRespostaService respostaService
        )
    {
        _checkListProcessService = checkListProcessService;
        _whatsAppService = whatsAppService;
        _clienteRepository = clienteRepository;
        _logger = logger;
        _interacaoRepository = interacaoRepository;
        _respostaService = respostaService;
    }

    public async Task EnviarCheckListParaClientesAsync(MensagemWppDTO mensagemWppDto)
    {
        foreach (var dado in mensagemWppDto.CheckListComTelefones)
        {
            try
            {
                _logger.LogInformation("Iniciando envio para o telefone {Telefone}.", dado.Telefone);

                var checkList = await _checkListProcessService.BuscarPorIdAsync(dado.CheckListId);
                if (checkList == null)
                {
                    _logger.LogWarning("Checklist com ID {CheckListId} não encontrado.", dado.CheckListId);
                    continue;
                }

                var cliente = await _clienteRepository.BuscarPorTelefone(dado.Telefone);
                if (cliente == null)
                {
                    _logger.LogWarning("Cliente com telefone {Telefone} não encontrado.", dado.Telefone);
                    continue;
                }

                await _checkListProcessService.ProcessarCheckListAsync(cliente.Id, checkList.Id);
                _logger.LogInformation("Checklist processado para o cliente {ClienteId}.", cliente.Id);

                var interacaoExist = await _interacaoRepository.BuscarPorIdCliente(cliente.Id);
                var qtdaQuestoesRespondidas = interacaoExist?.RespostaAlternativaModels?.Count ?? 0;
                
                var mensagem = GerarMensagem(checkList, qtdaQuestoesRespondidas, 0);
                var telefone = $"+55{dado.Telefone.Trim()}";
                
                await _whatsAppService.EnviarMensagemAsync(telefone, mensagem);

                _logger.LogInformation("Mensagem enviada com sucesso para o telefone {Telefone}.", telefone);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enviar mensagem para o telefone {Telefone}.", dado.Telefone);
            }
        }
    }

    private string GerarMensagem(CheckListModel checkList, int qtda, int flag)
    {
        var mensagem = "";
        var horarioAtual = DateTime.Now;

        if (horarioAtual.Hour < 12 && flag == 0)
            mensagem +=
                "Bom dia! ☀️ Aqui é a Nova Rota Oeste. Estamos enviando um checklist para ajudá-lo(a) a " +
                "verificar alguns detalhes importantes. Isso nos ajudará a garantir que tudo está em " +
                "ordem e que você receba o melhor serviço. Vamos começar? 😊\n\n";
        else if(horarioAtual.Hour > 12 && flag == 0)
            mensagem +=
                "Boa tarde! 🌞 Aqui é a Nova Rota Oeste." +
                " Esperamos que seu dia esteja indo bem! Estamos enviando um checklist rápido para que possamos coletar " +
                "algumas informações importantes e garantir o máximo de eficiência nos nossos serviços. Pronto para começar? 😊\n\n";

        mensagem += $"{checkList.Nome}\n\n";

        var questao = checkList.Questoes.ElementAtOrDefault(qtda);
        if (questao != null)
        {
            mensagem += $"{questao.Titulo}\n";
            foreach (var alternativa in questao.AlternativaModels)
            {
                mensagem += $"{alternativa.Codigo}: {alternativa.Descricao}\n";
            }
        }
        else
        {
            mensagem += "Não há mais questões para responder neste checklist.\n";
        }

        return mensagem;
    }

    public async Task<bool> VerificarMensagemAsync(string requestFrom, string requestBody)
    {
        var telefone = requestFrom.Substring(3);
        var cliente = _clienteRepository.BuscarPorTelefone(telefone);
        
        if (cliente == null)
        {
            _logger.LogWarning("Cliente com telefone {Telefone} não encontrado.", requestFrom);
            return false;
        }        
        
        var interacao = await _interacaoRepository.BuscarPorIdCliente(cliente.Id);

        var qtdaRespostas = interacao.RespostaAlternativaModels.Count;
        
        var proxQuestaoLocalizada = interacao.CheckList.Questoes.ElementAtOrDefault(qtdaRespostas);
        
        return AvaliarTipoQuestao(qtdaRespostas, proxQuestaoLocalizada, interacao, requestBody);

    }
    

    public bool AvaliarTipoQuestao(int qtda, QuestaoModel proxQuestaoLocalizada, InteracaoModel interacao, string requestBody)
    {

        var resultadoRegex = ValidarResposta(requestBody);
        if(!resultadoRegex)
            return false;
        
        if (proxQuestaoLocalizada.Tipo == TipoQuestao.QUESTAO_OBJETIVA)
        {
            requestBody = requestBody.Trim();
            
            var alternativaLocalizada = 0;
            
            foreach (var alternativa in proxQuestaoLocalizada.AlternativaModels)
            {
                if (alternativa.Codigo.ToString() == requestBody)
                {
                    alternativaLocalizada = alternativa.Id;
                    break;
                }
            }

            var novaResposta = new RespostaRequestDTO(
                proxQuestaoLocalizada.Id,
                interacao.Id,
                null
                );
            
            var resultadoResposta =_respostaService.AdicionarAsync(novaResposta);
            
            _respostaService
                .AdicionarRespostaTemAlternativaAsync(resultadoResposta.Id, alternativaLocalizada);
            
        }
        else if (proxQuestaoLocalizada.Tipo == TipoQuestao.QUESTAO_MULTIPLA_ESCOLHA)
        {
            var respostasAlternativas = requestBody.Split(' ').ToList();

            var index = 0;
            
            var alternativasLocalizada = new List<int>();
            
            foreach (var alternativa in proxQuestaoLocalizada.AlternativaModels)
            {
                if (alternativa.Codigo.ToString() == respostasAlternativas[index])
                {
                    alternativasLocalizada.Add(alternativa.Id);
                    alternativasLocalizada.RemoveAt(index);
                    index += 1;
                    if (alternativasLocalizada.Count == 0)
                        break;
                }
                
            }

            // if (alternativasLocalizada.Count != 0)
            //     return false;
            
            var novaResposta = new RespostaRequestDTO(
                proxQuestaoLocalizada.Id,
                interacao.Id,
                null
            );
            
            var resultadoResposta =_respostaService.AdicionarAsync(novaResposta);

            foreach (var alternativa in alternativasLocalizada)
            {
                _respostaService
                    .AdicionarRespostaTemAlternativaAsync(resultadoResposta.Id, alternativa);
            }

        }

        if (interacao.CheckList != null) GerarMensagem(interacao.CheckList, qtda + 1, 1);

        return true;
    }
    
    public bool ValidarResposta(string resposta)
    {
        string pattern = @"^(\d{1,5})( \d{1,5})*$";
        return Regex.IsMatch(resposta, pattern);
    }

    
}
