using System.Text;
using api_rota_oeste.Services.Interfaces;
using Newtonsoft.Json;

namespace api_rota_oeste.Services
{
    public class WhatsAppService : IWhatsAppService
    {
        private readonly HttpClient _httpClient;

        public WhatsAppService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task EnviarMensagemWhatsAppAsync(string telefone, string mensagemTexto)
        {
            var mensagem = new
            {
                messaging_product = "whatsapp",
                to = telefone,
                type = "text",
                text = new
                {
                    body = mensagemTexto
                }
            };

            var jsonMensagem = JsonConvert.SerializeObject(mensagem);
            var content = new StringContent(jsonMensagem, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("", content); // URL base já configurada
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Mensagem enviada com sucesso!");
                }
                else
                {
                    Console.WriteLine($"Erro ao enviar mensagem: {response.StatusCode}");
                    Console.WriteLine($"Detalhes do erro: {responseContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar a solicitação: {ex.Message}");
            }
        }
    }
}
