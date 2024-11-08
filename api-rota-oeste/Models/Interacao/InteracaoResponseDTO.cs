using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.RespostaAlternativa;

namespace api_rota_oeste.Models.Interacao;

/// <summary>
/// Representa o DTO <see cref="InteracaoResponseDTO"/> que descreve os dados de resposta de uma interação.
/// </summary>
/// <remarks>
/// Este DTO é utilizado para transferir informações detalhadas de uma interação, incluindo o identificador, cliente, checklist, status e as respostas alternativas associadas.
/// </remarks>
/// <example>
/// Aqui está um exemplo de como usar o <see cref="InteracaoResponseDTO"/>:
/// <code>
/// var interacaoResponse = new InteracaoResponseDTO(1, 1, 2, true, clienteResponse, checkListResponse, respostaAlternativaModels);
/// </code>
/// </example>
/// <seealso cref="ClienteResponseDTO"/>
/// <seealso cref="CheckListResponseDTO"/>
/// <seealso cref="RespostaResponseDTO"/>
public record InteracaoResponseDTO
{
    public int Id { get; init; }
    public int ClienteId { get; init; }
    public int CheckListId { get; init; }
    public bool Status { get; init; }
    public ClienteResponseDTO? Cliente { get; init; }
    public CheckListResponseDTO? CheckList { get; init; }
    public List<RespostaResponseDTO>? RespostaAlternativaModels { get; init; }

    // Construtor vazio para AutoMapper
    public InteracaoResponseDTO() {}

    // Construtor principal
    public InteracaoResponseDTO(
        int id,
        int clienteId,
        int checkListId,
        bool status,
        ClienteResponseDTO? cliente,
        CheckListResponseDTO? checkList,
        List<RespostaResponseDTO>? respostaAlternativaModels)
    {
        Id = id;
        ClienteId = clienteId;
        CheckListId = checkListId;
        Status = status;
        Cliente = cliente;
        CheckList = checkList;
        RespostaAlternativaModels = respostaAlternativaModels;
    }
}