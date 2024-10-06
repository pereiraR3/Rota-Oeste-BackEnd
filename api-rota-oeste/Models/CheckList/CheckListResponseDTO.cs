using api_rota_oeste.Models.ClienteRespondeCheckList;
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Models.Usuario;

namespace api_rota_oeste.Models.CheckList;

/// <summary>
/// Representa o DTO <see cref="CheckListResponseDTO"/> que descreve os dados de resposta de um checklist.
/// </summary>
/// <remarks>
/// Este DTO é utilizado para transferir os dados detalhados de um checklist, incluindo o identificador, nome, data de criação, questões associadas, usuário responsável e os clientes que responderam ao checklist.
/// </remarks>
/// <example>
/// Aqui está um exemplo de como usar o <see cref="CheckListResponseDTO"/>:
/// <code>
/// var checklistResponse = new CheckListResponseDTO(1, 1, "Nome do Checklist", DateTime.Now, questoes, usuario, clientesResponde);
/// </code>
/// </example>
/// <seealso cref="QuestaoResponseDTO"/>
/// <seealso cref="UsuarioResponseDTO"/>
/// <seealso cref="ClienteRespondeCheckListModel"/>
public record CheckListResponseDTO
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string Nome { get; set; }
    public DateTime? DataCriacao { get; set; }
    public List<QuestaoResponseDTO>? Questoes { get; set; }
    public UsuarioResponseDTO? Usuario { get; set; }
    public List<ClienteRespondeCheckListModel>? ClienteResponde { get; set; }

    // Construtor sem parâmetros
    public CheckListResponseDTO() {}

    // Construtor com parâmetros
    public CheckListResponseDTO(int id, int usuarioId, string nome, DateTime? dataCriacao, List<QuestaoResponseDTO>? questoes, UsuarioResponseDTO? usuario, List<ClienteRespondeCheckListModel>? clienteResponde)
    {
        Id = id;
        UsuarioId = usuarioId;
        Nome = nome;
        DataCriacao = dataCriacao;
        Questoes = questoes;
        Usuario = usuario;
        ClienteResponde = clienteResponde;
    }
}