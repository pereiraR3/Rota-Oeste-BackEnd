using api_rota_oeste.Models.ClienteRespondeCheckList;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Models.Usuario;

namespace api_rota_oeste.Models.Cliente;

public record ClienteResponseDTO
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string Nome { get; set; }
    public string Telefone { get; set; }
    public byte[]? Foto { get; set; } // Ajustado para ser opcional

    public UsuarioResponseDTO? Usuario { get; set; }
    public List<InteracaoResponseDTO>? Interacoes { get; set; }
    public List<ClienteRespondeCheckListModel>? ClienteResponde { get; set; }

    // Construtor padrão necessário para compatibilidade com mapeamento automático
    public ClienteResponseDTO() {}

    // Construtor opcional, permitindo inicialização simplificada
    public ClienteResponseDTO(int id, int usuarioId, string nome, string telefone, byte[]? foto = null)
    {
        Id = id;
        UsuarioId = usuarioId;
        Nome = nome;
        Telefone = telefone;
        Foto = foto;
    }
}