namespace api_rota_oeste.Services.Interfaces;

public interface IWhatsAppService
{
    Task EnviarMensagemAsync(string toPhoneNumber, string message);

}