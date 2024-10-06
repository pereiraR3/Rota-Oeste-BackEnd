using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Cliente;

namespace api_rota_oeste.Models.Usuario;

/// <summary>
/// Representa o DTO <see cref="UsuarioResponseDTO"/> que descreve os dados de resposta de um usuário.
/// </summary>
/// <remarks>
/// Este DTO é utilizado para transferir informações detalhadas de um usuário, incluindo o identificador, telefone, nome, foto, e as listas de clientes e checklists associados.
/// </remarks>
/// <example>
/// Aqui está um exemplo de como usar o <see cref="UsuarioResponseDTO"/>:
/// <code>
/// var usuarioResponse = new UsuarioResponseDTO(1, "12345678901", "Nome do Usuário", fotoBytes, clientes, checklists);
/// </code>
/// </example>
/// <seealso cref="ClienteResponseDTO"/>
/// <seealso cref="CheckListResponseDTO"/>
public record UsuarioResponseDTO(   
        int Id,
        
        string Telefone,
        
        string Nome, 
        
        byte[] Foto,
        
        List<ClienteResponseDTO>? Clientes,
        
        List<CheckListResponseDTO>? CheckLists
        
);