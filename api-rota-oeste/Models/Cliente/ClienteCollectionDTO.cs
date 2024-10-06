namespace api_rota_oeste.Models.Cliente;

/// <summary>
/// Representa o DTO <see cref="ClienteCollectionDTO"/> que descreve uma coleção de clientes para criação.
/// </summary>
/// <remarks>
/// Este DTO é utilizado para transferir uma lista de clientes, geralmente usado em operações que envolvem múltiplos clientes ao mesmo tempo.
/// </remarks>
/// <example>
/// Aqui está um exemplo de como usar o <see cref="ClienteCollectionDTO"/>:
/// <code>
/// var clienteCollection = new ClienteCollectionDTO(new List<ClienteRequestDTO>
/// {
///     new ClienteRequestDTO(1, "Cliente 1", "12345678901", foto1),
///     new ClienteRequestDTO(2, "Cliente 2", "10987654321", foto2)
/// });
/// </code>
/// </example>
/// <seealso cref="ClienteRequestDTO"/>
public record ClienteCollectionDTO(
    
    List<ClienteRequestDTO> Clientes

    );