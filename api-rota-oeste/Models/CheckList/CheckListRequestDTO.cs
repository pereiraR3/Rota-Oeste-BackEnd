namespace api_rota_oeste.Models.CheckList;

/// <summary>
/// Representa o DTO <see cref="CheckListRequestDTO"/> que descreve os campos necessários para a criação de um novo checklist.
/// </summary>
/// <remarks>
/// Este DTO é utilizado para transferir dados ao criar um novo checklist, incluindo o identificador do usuário e o nome do checklist.
/// </remarks>
/// <example>
/// Aqui está um exemplo de como usar o <see cref="CheckListRequestDTO"/>:
/// <code>
/// var checklistRequest = new CheckListRequestDTO(1, "Nome do Checklist");
/// </code>
/// </example>
public record CheckListRequestDTO(
    
    int UsuarioId,
        
    string? Nome
    
);
