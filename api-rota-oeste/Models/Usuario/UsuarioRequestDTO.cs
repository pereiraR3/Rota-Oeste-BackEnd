namespace api_rota_oeste.Models.Usuario;

/// <summary>
/// Representa o DTO <see cref="UsuarioRequestDTO"/> que descreve os campos necessários para a criação de um novo usuário.
/// </summary>
/// <remarks>
/// Este DTO é utilizado para transferir os dados ao criar um novo usuário, incluindo telefone, nome, senha e foto.
/// </remarks>
/// <example>
/// Aqui está um exemplo de como usar o <see cref="UsuarioRequestDTO"/>:
/// <code>
/// var usuarioRequest = new UsuarioRequestDTO("12345678901", "Nome do Usuário", "SenhaSegura123", fotoBytes);
/// </code>
/// </example>
public record UsuarioRequestDTO(
    
    string Telefone, 
    
    string Nome,
    
    string Senha,
    
    byte[] Foto
    
);