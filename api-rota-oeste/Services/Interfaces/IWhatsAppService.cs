
namespace api_rota_oeste.Services.Interfaces;

public interface IWhatsAppService
{
    Task EnviarMensagemWhatsAppAsync(string telefone, string templateName);
    
}