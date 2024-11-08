using api_rota_oeste.Models.WppMessage;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using api_rota_oeste.Services.Interfaces;
using api_rota_oeste.Services.Utils;

namespace api_rota_oeste.Controllers
{
    /// <summary>
    /// Controller responsável por operações de envio de mensagens via WhatsApp.
    /// </summary>
    /// <remarks>
    /// Esta controller fornece endpoints para envio de mensagens únicas ou em massa via WhatsApp,
    /// utilizando a API do WhatsApp Business.
    /// </remarks>
    [ApiController]
    [Route("api/whatsapp")]
    public class WhatsAppController : ControllerBase
    {
        private readonly IWhatsAppService _whatsAppService;
        private readonly ILogger<WhatsAppController> _logger;
        private readonly MessageOrchestrationService _messageOrchestrationService;
        public WhatsAppController(
            IWhatsAppService whatsAppService,
            ILogger<WhatsAppController> logger,
            MessageOrchestrationService messageOrchestrationService
            )
        {
            _whatsAppService = whatsAppService;
            _logger = logger;
            _messageOrchestrationService = messageOrchestrationService;
        }
        
        /// <summary>
        /// Envia uma mensagem via WhatsApp para um número de telefone específico.
        /// </summary>
        /// <param name="request">Objeto contendo o número de telefone e a mensagem a ser enviada.</param>
        /// <returns>Retorna o status do envio da mensagem.</returns>
        /// <response code="200">Mensagem enviada com sucesso.</response>
        /// <response code="400">Erro nos dados fornecidos.</response>
        /// <response code="500">Erro ao enviar a mensagem.</response>
        [HttpPost("enviar")]
        [SwaggerOperation(
            Summary = "Envia uma mensagem via WhatsApp",
            Description = "Envia uma mensagem para um número de telefone específico via WhatsApp, utilizando a API do WhatsApp Business."
        )]
        [SwaggerResponse(200, "Mensagem enviada com sucesso.")]
        [SwaggerResponse(400, "Erro nos dados fornecidos.")]
        [SwaggerResponse(500, "Erro ao enviar a mensagem.")]
        public async Task<IActionResult> EnviarMensagemUnica([FromBody] EnviarMensagemRequest request)
        {
            if (string.IsNullOrEmpty(request.Telefone) || string.IsNullOrEmpty(request.Mensagem))
            {
                return BadRequest("Número de telefone e mensagem são obrigatórios.");
            }

            try
            {
                await _whatsAppService.EnviarMensagemAsync(request.Telefone, request.Mensagem);
                _logger.LogInformation("Mensagem enviada com sucesso para {Telefone}.", request.Telefone);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enviar a mensagem para {Telefone}.", request.Telefone);
                return StatusCode(500, $"Erro ao enviar a mensagem: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Envia mensagens de checklist em massa para múltiplos destinatários via WhatsApp.
        /// </summary>
        /// <param name="mensagemWppDto">Objeto contendo a lista de checklists e números de telefone.</param>
        /// <returns>Retorna o status do envio das mensagens.</returns>
        /// <response code="200">Mensagens enviadas com sucesso.</response>
        /// <response code="400">Erro nos dados fornecidos.</response>
        /// <response code="500">Erro ao enviar as mensagens.</response>
        [HttpPost("enviar-em-massa")]
        [SwaggerOperation(
            Summary = "Envia mensagens de checklist em massa",
            Description = "Envia mensagens de checklist para múltiplos números de telefone via WhatsApp, utilizando a API do WhatsApp Business."
        )]
        [SwaggerResponse(200, "Mensagens enviadas com sucesso.")]
        [SwaggerResponse(400, "Erro nos dados fornecidos.")]
        [SwaggerResponse(500, "Erro ao enviar as mensagens.")]
        public async Task<IActionResult> EnviarMensagemEmMassa([FromBody] MensagemWppDTO mensagemWppDto)
        {
            if (mensagemWppDto?.CheckListComTelefones == null || mensagemWppDto.CheckListComTelefones.Count == 0)
            {
                return BadRequest("A lista de checklists e telefones não pode estar vazia.");
            }

            try
            {
                await _messageOrchestrationService.EnviarCheckListParaClientesAsync(mensagemWppDto);
                _logger.LogInformation("Mensagens de checklist enviadas com sucesso.");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enviar as mensagens em massa.");
                return StatusCode(500, $"Erro ao enviar as mensagens: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Recebe e valida uma mensagem do cliente enviada via WhatsApp.
        /// </summary>
        /// <param name="request">Dados da mensagem recebida.</param>
        /// <returns>Retorna um status de sucesso ou erro com base na validação da mensagem.</returns>
        /// <response code="200">Mensagem recebida e validada com sucesso.</response>
        /// <response code="400">Erro na validação dos dados recebidos.</response>
        [HttpPost("receber")]
        [SwaggerOperation(
            Summary = "Recebe mensagem do cliente",
            Description = "Recebe uma mensagem enviada pelo cliente via WhatsApp e valida seu conteúdo."
        )]
        [SwaggerResponse(200, "Mensagem recebida e validada com sucesso.")]
        [SwaggerResponse(400, "Erro na validação dos dados recebidos.")]
        public async Task<IActionResult> ReceberMensagem([FromForm] WhatsAppRequest request)
        {
            if (string.IsNullOrEmpty(request.From) || string.IsNullOrEmpty(request.Body))
            {
                _logger.LogWarning("Dados de mensagem inválidos recebidos. Telefone e conteúdo da mensagem são obrigatórios.");
                return BadRequest("Telefone e conteúdo da mensagem são obrigatórios.");
            }

            try
            {
                _logger.LogInformation("Recebendo mensagem de {From}: {Body}", request.From, request.Body);

                // Valida a mensagem usando um método no serviço de WhatsApp
                bool isValid = await _messageOrchestrationService.VerificarMensagemAsync(request.From, request.Body);

                if (isValid)
                {
                    _logger.LogInformation("Mensagem validada com sucesso de {From}.", request.From);
                    return Ok();
                }
                else
                {
                    _logger.LogWarning("Mensagem de {From} não passou na validação.", request.From);
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar a mensagem de {From}.", request.From);
                return StatusCode(500, $"Erro ao processar a mensagem: {ex.Message}");
            }
        }
    }
}

public class EnviarMensagemRequest
{
    /// <summary>
    /// Número de telefone do destinatário, incluindo o código do país.
    /// </summary>
    public string Telefone { get; set; }

    /// <summary>
    /// Conteúdo da mensagem a ser enviada.
    /// </summary>
    public string Mensagem { get; set; }
}

public class WhatsAppRequest
{
    /// <summary>
    /// Identificador único da mensagem enviada pelo Twilio.
    /// </summary>
    public string SmsSid { get; set; }

    /// <summary>
    /// Número de telefone do remetente.
    /// </summary>
    public string From { get; set; }

    /// <summary>
    /// Conteúdo da mensagem recebida.
    /// </summary>
    public string Body { get; set; }
}
