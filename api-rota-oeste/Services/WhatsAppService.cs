using System.Text;
using api_rota_oeste.Services.Interfaces;
using Newtonsoft.Json;

namespace api_rota_oeste.Services

{
    
    /// <summary>
    /// Serviço responsável por enviar mensagens via WhatsApp utilizando a API do WhatsApp Business.
    /// </summary>
    /// <remarks>
    /// Implementa a interface <see cref="IWhatsAppService"/> e define um método para enviar mensagens de texto.
    /// Utiliza o <see cref="HttpClient"/> injetado para fazer a solicitação HTTP.
    /// </remarks>
    public class WhatsAppService : IWhatsAppService
    {
        private readonly HttpClient _httpClient;

        public WhatsAppService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Envia uma mensagem de texto para um número de telefone específico via WhatsApp.
        /// </summary>
        /// <param name="telefone">O número de telefone do destinatário da mensagem, no formato internacional.</param>
        /// <param name="mensagemTexto">O conteúdo da mensagem de texto a ser enviado.</param>
        /// <returns>Retorna uma <see cref="Task"/> que representa a operação assíncrona.</returns>
        /// <remarks>
        /// Este método utiliza a API do WhatsApp Business para enviar uma mensagem para um número de telefone.
        /// O corpo da mensagem é convertido para JSON e enviado como uma solicitação POST.
        /// </remarks>
        /// <exception cref="Exception">Captura qualquer exceção durante o envio da solicitação e exibe a mensagem de erro no console.</exception>
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
