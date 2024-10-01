
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Models.Usuario;

namespace api_rota_oeste.Models.CheckList;

public record CheckListResponseDTO
(
    int Id,
    
    int UsuarioId,
    
    string Nome,
    
    DateTime? DataCriacao,
    
    List<QuestaoResponseDTO>? Questoes,
    
    UsuarioResponseDTO? Usuario
    
);
