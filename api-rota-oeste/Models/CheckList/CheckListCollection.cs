namespace api_rota_oeste.Models.CheckList;

/// <summary>
/// Representa o DTO <see cref="CheckListCollectionDTO"/> que descreve uma coleção de checklists para criação.
/// </summary>
/// <remarks>
/// Este DTO é utilizado para transferir uma lista de checklists, geralmente usada em operações que envolvem a criação de múltiplos checklists ao mesmo tempo.
/// </remarks>
/// <example>
/// Aqui está um exemplo de como usar o <see cref="CheckListCollectionDTO"/>:
/// <code>
/// var checklistCollection = new CheckListCollectionDTO(new List<CheckListRequestDTO> {
///     new CheckListRequestDTO(1, "Checklist 1"),
///     new CheckListRequestDTO(2, "Checklist 2")
/// });
/// </code>
/// </example>
/// <seealso cref="CheckListRequestDTO"/>
public record CheckListCollectionDTO(
        
    List<CheckListRequestDTO> CheckLists
    
);

