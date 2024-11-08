using api_rota_oeste.Models.CheckList;
using Swashbuckle.AspNetCore.Annotations;

namespace api_rota_oeste.Models.WppMessage
{
    /// <summary>
    /// DTO para enviar mensagens de checklist via WhatsApp para múltiplos destinatários.
    /// </summary>
    public record MensagemWppDTO(
        [property: SwaggerSchema(Description = "Lista de destinatários e seus respectivos checklists")]
        List<InfoCliente> CheckListComTelefones
    );

    /// <summary>
    /// Contém o número de telefone e o checklist associado a um destinatário.
    /// </summary>
    /// </summary>
    public record InfoCliente(
        [property: SwaggerSchema(Description = "O objeto de cliente vem pegando a referencia de Id")]
        int clienteId,

        [property: SwaggerSchema(Description = "Objeto de checklist associado ao destinatário, contendo as perguntas e alternativas.")]
        int CheckListId
    );
}