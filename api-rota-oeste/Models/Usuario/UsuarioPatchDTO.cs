namespace api_rota_oeste.Models.Usuario;

/// <summary>
/// Representa o DTO <see cref="UsuarioPatchDTO"/> que descreve os campos para atualização parcial de um usuário.
/// </summary>
/// <remarks>
/// Este DTO é utilizado para transferir dados ao atualizar parcialmente um usuário, incluindo o telefone, nome e foto.
/// </remarks>
/// <example>
/// Aqui está um exemplo de como usar o <see cref="UsuarioPatchDTO"/>:
/// <code>
/// var usuarioPatch = new UsuarioPatchDTO
/// {
///     Id = 1,
///     Nome = "Novo Nome do Usuário",
///     Telefone = "0987654321"
/// };
/// </code>
/// </example>
public record UsuarioPatchDTO
{
    public int Id { get; init; }
    public string? Telefone { get; init; }
    public string? Nome { get; init; }
    public byte[]? Foto { get; init; }
}