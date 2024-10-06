
namespace api_rota_oeste.Models.Cliente;

/// <summary>
/// Representa o DTO <see cref="ClienteRequestDTO"/> que descreve os campos necessários para a criação de um novo cliente.
/// </summary>
/// <remarks>
/// Este DTO é utilizado para transferir dados de um cliente, incluindo o identificador do usuário associado, o nome, telefone e a foto do cliente.
/// </remarks>
/// <example>
/// Aqui está um exemplo de como usar o <see cref="ClienteRequestDTO"/>:
/// <code>
/// var clienteRequest = new ClienteRequestDTO(1, "Nome do Cliente", "12345678901", fotoBytes);
/// </code>
/// </example>
public record ClienteRequestDTO
(
    
    int UsuarioId,
    
    string Nome,
    
    string Telefone, 
    
    byte[]? Foto
    
);
