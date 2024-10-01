using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using api_rota_oeste.Services.Interfaces;

namespace api_rota_oeste.Controllers
{
    [ApiController]
    [Route("whatsapp")]
    public class WhatsAppController : ControllerBase
    {
        private readonly IWhatsAppService _whatsAppService;

        public WhatsAppController(IWhatsAppService whatsAppService)
        {
            _whatsAppService = whatsAppService;
        }
        
        [HttpPost("enviar")]
        [SwaggerOperation(
            Summary = "Envia uma mensagem via WhatsApp",
            Description = "Envia uma mensagem para um número de telefone específico via WhatsApp, utilizando a API do WhatsApp Business."
        )]
        [SwaggerResponse(200, "Mensagem enviada com sucesso")]
        [SwaggerResponse(400, "Erro nos dados fornecidos")]
        [SwaggerResponse(500, "Erro ao enviar a mensagem")]
        public async Task<IActionResult> EnviarMensagem([FromBody] EnviarMensagemRequest request)
        {
            try
            {
                await _whatsAppService.EnviarMensagemWhatsAppAsync(request.Telefone, request.Mensagem);
                return Ok("Mensagem enviada com sucesso");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao enviar a mensagem: {ex.Message}");
            }
        }
    }
}


public class EnviarMensagemRequest
{
    public string Telefone { get; set; }
    public string Mensagem { get; set; }
    
}
