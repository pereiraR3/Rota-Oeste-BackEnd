using api_rota_oeste.Services.Interfaces;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace api_rota_oeste.Services
{
    public class WhatsAppService : IWhatsAppService
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _whatsAppNumber;

        public WhatsAppService(IConfiguration configuration)
        {
            _accountSid = configuration["Twilio:AccountSid"];
            _authToken = configuration["Twilio:AuthToken"];
            _whatsAppNumber = configuration["Twilio:WhatsAppNumber"];

            // Inicializa o cliente Twilio
            TwilioClient.Init(_accountSid, _authToken);
        }

        public async Task EnviarMensagemAsync(string toPhoneNumber, string message)
        {
            var messageOptions = new CreateMessageOptions(
                new PhoneNumber($"whatsapp:{toPhoneNumber}")
            )
            {
                From = new PhoneNumber(_whatsAppNumber),
                Body = message
            };

            await MessageResource.CreateAsync(messageOptions);
        }
    }
}