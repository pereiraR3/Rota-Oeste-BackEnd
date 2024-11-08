using api_rota_oeste.Models.Alternativa;
using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Models.WppMessage;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Text.RegularExpressions;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Models.RespostaAlternativa;
using api_rota_oeste.Models.RespostaTemAlternativa;

namespace api_rota_oeste.Services.Utils;

public class MessageOrchestrationService
{
    private readonly ICheckListProcessService _checkListProcessService;
    private readonly IWhatsAppService _whatsAppService;
    private readonly IClienteRepository _clienteRepository;
    private readonly ILogger<MessageOrchestrationService> _logger;
    private readonly IInteracaoRepository _interacaoRepository;
    private readonly IRespostaService _respostaService;
    private readonly IClienteRespondeCheckListRepository _clienteRespondeCheckListRepository;
    private readonly IQuestaoService _questaoService;
    private readonly IRespostaTemAlternativaRepository _respostaTemAlternativaRepository;
    private readonly IRepository _repository;

    public MessageOrchestrationService(
        
        ICheckListProcessService checkListProcessService, 
        IWhatsAppService whatsAppService, 
        IClienteRepository clienteRepository, 
        ILogger<MessageOrchestrationService> logger, 
        IInteracaoRepository interacaoRepository,
        IRespostaService respostaService,
        IClienteRespondeCheckListRepository clienteRespondeCheckListRepository, IQuestaoService questaoService, IRespostaTemAlternativaRepository respostaTemAlternativaRepository, IRepository repository)
    {
        _checkListProcessService = checkListProcessService;
        _whatsAppService = whatsAppService;
        _clienteRepository = clienteRepository;
        _logger = logger;
        _interacaoRepository = interacaoRepository;
        _respostaService = respostaService;
        _clienteRespondeCheckListRepository = clienteRespondeCheckListRepository;
        _questaoService = questaoService;
        _respostaTemAlternativaRepository = respostaTemAlternativaRepository;
        _repository = repository;
    }

    public async Task EnviarCheckListParaClientesAsync(MensagemWppDTO mensagemWppDto)
    {
        var tasks = new List<Task>();

        foreach (var dado in mensagemWppDto.CheckListComTelefones)
        {
                try
                {
                    _logger.LogInformation("Iniciando envio para o telefone {id}.", dado.clienteId);

                    var checkList = await _checkListProcessService.BuscarPorIdAsync(dado.CheckListId);
                    if (checkList == null)
                    {
                        _logger.LogWarning("Checklist com ID {CheckListId} não encontrado.", dado.CheckListId);
                        return;
                    }

                    var cliente = await _clienteRepository.BuscarPorId(dado.clienteId);
                    if (cliente == null)
                    {
                        _logger.LogWarning("Cliente com telefone {Telefone} não encontrado.", cliente.Telefone);
                        return;
                    }

                    // Processa o checklist para o cliente
                    await _checkListProcessService.ProcessarCheckListAsync(cliente.Id, checkList.Id);
                    _logger.LogInformation("Checklist processado para o cliente {ClienteId}.", cliente.Id);

                    var interacaoExist = await _interacaoRepository.BuscarPorIdCliente(cliente.Id);
                    if (interacaoExist == null)
                    {
                        _logger.LogWarning("Interação não encontrada para o cliente {ClienteId}.", cliente.Id);
                        return;
                    }

                    int qtdaQuestoesRespondidas = interacaoExist.RespostaModels.Count;
                    
                    string mensagem;

                    // Gera a mensagem para a próxima pergunta
                    mensagem = await GerarMensagem(checkList, qtdaQuestoesRespondidas, 0);
                    await _whatsAppService.EnviarMensagemAsync(FormatarNumero(cliente.Telefone), mensagem); // Certifique-se de que isso está sendo chamado
                    _logger.LogInformation("Mensagem enviada com sucesso para o telefone {Telefone}.", FormatarNumero(cliente.Telefone));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao enviar mensagem para o telefone {id}.", dado.clienteId);
                }
        }

    }

    private async Task<string> GerarMensagem(CheckListModel checkList, int qtda, int flag)
    {
        
        var mensagem = "";
            
        var horarioAtual = DateTime.Now;
        
        if (horarioAtual.Hour < 12 && flag == 0)
            mensagem +=
                "Bom dia! ☀️ Aqui é a Nova Rota Oeste. Estamos enviando um checklist para ajudá-lo(a) a " +
                "verificar alguns detalhes importantes. Isso nos ajudará a garantir que tudo está em " +
                "ordem e que você receba o melhor serviço. Vamos começar? 😊\n\n";
        else if(horarioAtual.Hour > 12 && horarioAtual.Hour < 18 && flag == 0)
            mensagem +=
                "Boa tarde! 🌞 Aqui é a Nova Rota Oeste." +
                " Esperamos que seu dia esteja indo bem! Estamos enviando um checklist rápido para que possamos coletar " +
                "algumas informações importantes e garantir o máximo de eficiência nos nossos serviços. Pronto para começar? 😊\n\n";

        if(flag == 0)
            mensagem += $"{checkList.Nome}\n\n";

        if (checkList.Questoes != null)
        {
            var questao = checkList.Questoes[qtda];
        
            _logger.LogInformation("Nome da Questão {NomeQuestao}.", questao.Titulo);
            
            var questaoNaveg = await _questaoService.BuscarPorIdAsync(questao.Id);
            
            mensagem += $"{questao.Titulo}\n";
            if (questaoNaveg.AlternativaModels != null && questaoNaveg.AlternativaModels.Count > 0)
            {
                foreach (var alternativa in questaoNaveg.AlternativaModels)
                {
                    mensagem += $"{alternativa.Codigo}: {alternativa.Descricao}\n";
                }
            }
            else
            {
                mensagem += "Alternativas Indisponíveis";
            }

            if (questao.Tipo == TipoQuestao.QUESTAO_OBJETIVA)
            {
                mensagem += "\n\nObservação: Responda com apenas um número de acordo com uma das alternativas 😊.";
            }
            else if (questao.Tipo == TipoQuestao.QUESTAO_MULTIPLA_ESCOLHA)
            {
                mensagem += "\n\nObservação: Responda com uma sequência de números separados por *espaços*, sem repetir valores 😊.";
            }
            
        }
 
        return mensagem;
    }

    public async Task<bool> VerificarMensagemAsync(string requestFrom, string requestBody)
    {
        var telefone = FormatarNumero(requestFrom).Substring(3);
        var cliente = await _clienteRepository.BuscarPorTelefone(telefone);
        
        if (cliente == null)
        {
            _logger.LogWarning("Cliente com telefone {Telefone} não encontrado.", requestFrom);
            return false;
        }        
        
        var interacao = await _interacaoRepository.BuscarPorIdCliente(cliente.Id);

        var qtdaRespostas = interacao.RespostaModels.Count;

        _logger.LogInformation("Quantidade de Respostas em Interação {n}", qtdaRespostas);
        
        _logger.LogInformation("Quantidade de Questoes em CheckList {n}", interacao.CheckList.Questoes.Count);
        
        if (qtdaRespostas == interacao.CheckList.Questoes.Count)
        {
            await _whatsAppService.EnviarMensagemAsync(FormatarNumero(requestFrom),
                "Você já terminou o CheckList, obrigado pela participação.");
            return true;
        }

        var proxQuestaoLocalizada = interacao.CheckList.Questoes.ElementAtOrDefault(qtdaRespostas);
        
        return await AvaliarTipoQuestao(qtdaRespostas, proxQuestaoLocalizada, interacao, requestBody, requestFrom);

    }
    
    public async Task<bool> AvaliarTipoQuestao(
        
        int qtda,
        QuestaoModel proxQuestaoLocalizada,
        InteracaoModel interacao,
        string requestBody,
        string requestFrom
        
        )
    {
        
        _logger.LogInformation("Entrou em AvaliarTipoQuestao");
        
        if (proxQuestaoLocalizada.Tipo == TipoQuestao.QUESTAO_OBJETIVA)
        {


            if (ValidarRespostaObjetiva(requestBody) && int.TryParse(requestBody, out int numero) && numero <= 5)
            {

                if (!proxQuestaoLocalizada.AlternativaModels.Any(x => x.Codigo == int.Parse(requestBody.Trim())))
                {
                    await _whatsAppService.EnviarMensagemAsync(FormatarNumero(requestFrom),
                        $"A alternativa {requestBody} não existe, tente novamente! \u274c");
                    return false;
                }
            }

            _logger.LogInformation("Entrou em Tipo de Questão Objetiva");

            if (!ValidarRespostaObjetiva(requestBody))
            {               
                await _whatsAppService.EnviarMensagemAsync(FormatarNumero(requestFrom), "Resposta inválida, por favor se atente à observação da questão! \ud83d\ude09");
                return false;
            }

            requestBody = requestBody.Trim();
            
            var alternativaLocalizadaId = 0;
            
            foreach (var alternativa in proxQuestaoLocalizada.AlternativaModels)
            {
                if (alternativa.Codigo.ToString() == requestBody)
                {
                    alternativaLocalizadaId = alternativa.Id;
                    break;
                }
            }

            var novaResposta = new RespostaRequestDTO(
                proxQuestaoLocalizada.Id,
                interacao.Id,
                null
                );
            
            var resultadoResposta = await _respostaService.AdicionarAsync(novaResposta);
            
            await _respostaService
                .AdicionarRespostaTemAlternativaAsync(resultadoResposta.Id, alternativaLocalizadaId);
            
        }
        else if (proxQuestaoLocalizada.Tipo == TipoQuestao.QUESTAO_MULTIPLA_ESCOLHA)
        {
            
            _logger.LogInformation("Entrou em Tipo de Questão Múltipla Escolha");

            if (!ValidarRespostaMultiplaEscolha(requestBody))
            {
                await _whatsAppService.EnviarMensagemAsync(FormatarNumero(requestFrom), "Resposta inválida, por favor se atente à observação da questão! \ud83d\ude09");
                return false;
            }

            // Divide a resposta em números individuais
            var respostasAlternativas = requestBody.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => int.Parse(x))
                .ToList()
                .OrderBy(x => x)
                .ToList();
            
            _logger.LogInformation("Lista respostasAlternativas {list}", respostasAlternativas);

            // Verifica se todas as alternativas existem
            foreach (var numeroAlternativa in respostasAlternativas)
            {
                if (!proxQuestaoLocalizada.AlternativaModels.Any(x => x.Codigo == numeroAlternativa))
                {
                    await _whatsAppService.EnviarMensagemAsync(FormatarNumero(requestFrom),
                        $"A alternativa {numeroAlternativa} não existe, tente novamente! \u274c");
                    return false;
                }
            }

            var alternativasLocalizada = new List<int>();
            int index = 0;

            while (respostasAlternativas.Count > 0 && index < proxQuestaoLocalizada.AlternativaModels.Count)
            {
                var alternativa = proxQuestaoLocalizada.AlternativaModels[index];
    
                if (alternativa.Codigo == respostasAlternativas[0])
                {
                    _logger.LogInformation("Encontrou alternativa {id}",  alternativa.Id);

                    alternativasLocalizada.Add(alternativa.Id);
                    respostasAlternativas.RemoveAt(0); 

                    continue;
                }

                index++; 
            }
            
            
            
            _logger.LogInformation("Recolheu todas as alternativas qtda:{qtda}", alternativasLocalizada.Count);

            var novaResposta = new RespostaRequestDTO(
                proxQuestaoLocalizada.Id,
                interacao.Id,
                null
            );
            
            var resultadoResposta = await _respostaService.AdicionarAsync(novaResposta);

            foreach (var alternativa in alternativasLocalizada)
            {
                var newEntity = new RespostaTemAlternativaModel();
                newEntity.AlternativaId = alternativa;
                newEntity.RespostaId = resultadoResposta.Id;
                
                await _respostaTemAlternativaRepository.Adicionar(newEntity);
                
                // await _respostaService
                //     .AdicionarRespostaTemAlternativaAsync(resultadoResposta.Id, alternativa);
            }

        }

        _logger.LogInformation("Count de Questoes {q} - {qtda}", interacao.CheckList.Questoes.Count, qtda);
        
        if (interacao.CheckList.Questoes.Count <= qtda + 1)
            
        {
            _logger.LogInformation("Count de Questoes {q} - {qtda}", interacao.CheckList.Questoes.Count, qtda);

            _logger.LogInformation("Finalização de CheckList");

            _repository.Atualizar(true, interacao.Id);

            // Envio de mensagem de agradecimento e finalização
            await _whatsAppService.EnviarMensagemAsync(FormatarNumero(requestFrom), "Obrigado por finalizar o CheckList. Nós da Rota Oeste agradecemos a sua participação.");
        
            return true; // Retorna true para indicar finalização e evitar o envio de outra pergunta
        }
        else
        {
            _logger.LogInformation("Envio de próxima questão");
            
            // Envio da próxima pergunta, pois o checklist não está finalizado
            await _whatsAppService.EnviarMensagemAsync(FormatarNumero(requestFrom), "Recebemos a sua resposta com sucesso, obrigado.");

            var mensagem = GerarMensagem(interacao.CheckList, qtda + 1, 1);
            
            await _whatsAppService.EnviarMensagemAsync(FormatarNumero(requestFrom), await mensagem);
        }

        return true;
    }
    
    public static bool ValidarRespostaMultiplaEscolha(string resposta)
    {
        // Remove espaços extras e divide a resposta em números
        var numerosStr = resposta.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        // Verifica se não está vazio
        if (numerosStr.Length == 0)
            return false;

        var numeros = new List<int>();

        foreach (var numStr in numerosStr)
        {
            // Tenta converter cada parte em um inteiro entre 1 e 5
            if (!int.TryParse(numStr, out int numero) || numero < 1 || numero > 5)
            {
                return false;
            }
            numeros.Add(numero);
        }

        // Verifica se existem números duplicados
        return numeros.Distinct().Count() == numeros.Count;
    }


    public bool ValidarRespostaObjetiva(string resposta)
    {
        if (int.TryParse(resposta, out int numero))
        {
            if (numero >= 1 && numero <= 5)
            {
                _logger.LogInformation("Retorna true");

                return true;
            }
        }

        _logger.LogInformation("Retorna false");

        return false;
        
    }
    
    private string FormatarNumero(string telefone)
    {
        // Remove qualquer prefixo `whatsapp:` e caracteres não numéricos.
        telefone = Regex.Replace(telefone.Trim(), @"[^\d+]", string.Empty);

        if (!telefone.StartsWith("+55"))
        {
            telefone = $"+55{telefone}";
        }

        return telefone;
    }


    
}