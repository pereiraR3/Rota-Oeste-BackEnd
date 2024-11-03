
using api_rota_oeste.Models.WppMessage;

namespace api_rota_oeste.Services.Interfaces;

public interface IWhatsAppService
{
    Task EnviarMensagemAsync(string toPhoneNumber, string message);
    
    // Task EnviarCheckListAsync(MensagemWppDTO mensagemWppDto);
    //
    // Task<bool> VerificarMensagemAsync(string toPhoneNumber, string message);
    
}