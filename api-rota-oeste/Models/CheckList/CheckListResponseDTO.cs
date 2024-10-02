using api_rota_oeste.Models.ClienteRespondeCheckList;
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Models.Usuario;

namespace api_rota_oeste.Models.CheckList;

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