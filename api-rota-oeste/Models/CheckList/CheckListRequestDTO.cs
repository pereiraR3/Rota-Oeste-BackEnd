namespace api_rota_oeste.Models.CheckList;

public record CheckListRequestDTO(
    string? Nome,
    DateTime DataCriacao,
    int UsuarioId
);