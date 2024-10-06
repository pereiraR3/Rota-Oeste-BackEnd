using api_rota_oeste.Models.ClienteRespondeCheckList;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Models.Usuario;

namespace api_rota_oeste.Models.Cliente;

/// <summary>
/// Representa o DTO <see cref="ClienteResponseDTO"/> que descreve os dados de resposta de um cliente.
/// </summary>
/// <remarks>
/// Este DTO é utilizado para transferir informações detalhadas de um cliente, incluindo identificador, nome, telefone, foto, e as relações com o usuário associado, interações e checklists respondidos.
/// </remarks>
/// <example>
/// Aqui está um exemplo de como usar o <see cref="ClienteResponseDTO"/>:
/// <code>
/// var clienteResponse = new ClienteResponseDTO(1, 1, "Nome do Cliente", "12345678901", fotoBytes);
/// clienteResponse.Usuario = usuarioResponse;
/// clienteResponse.Interacoes = new List<InteracaoResponseDTO>();
/// </code>
/// </example>
/// <seealso cref="UsuarioResponseDTO"/>
/// <seealso cref="InteracaoResponseDTO"/>
/// <seealso cref="ClienteRespondeCheckListResponseDTO"/>
public record ClienteResponseDTO
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string Nome { get; set; }
    public string Telefone { get; set; }
    public byte[]? Foto { get; set; } // Ajustado para ser opcional

    public UsuarioResponseDTO? Usuario { get; set; }
    public List<InteracaoResponseDTO>? Interacoes { get; set; }
    public List<ClienteRespondeCheckListResponseDTO>? ClienteResponde { get; set; }

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