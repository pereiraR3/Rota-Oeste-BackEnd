namespace api_rota_oeste.Models.CheckList;

/// <summary>
/// Representa o DTO <see cref="CheckListPatchDTO"/> que descreve os campos para atualização parcial de um checklist.
/// </summary>
/// <remarks>
/// Este DTO é utilizado para transferir dados quando apenas alguns campos de um checklist precisam ser atualizados, como o nome.
/// </remarks>
/// <example>
/// Aqui está um exemplo de como usar o <see cref="CheckListPatchDTO"/>:
/// <code>
/// var checklistPatch = new CheckListPatchDTO(1, "Novo Nome do Checklist");
/// </code>
/// </example>
public record CheckListPatchDTO(

    int Id,
    
    string? Nome
    
);