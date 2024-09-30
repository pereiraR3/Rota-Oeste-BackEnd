using api_rota_oeste.Models.Interacao;

namespace api_rota_oeste.Repositories.Interfaces;

public interface IInteracaoRepository {
    Task <InteracaoModel> Criar(InteracaoRequestDTO req);

    void criar(InteracaoModel interacao);

    Task<InteracaoModel?> BuscarPorId(int id);

    Task<bool> Atualizar(InteracaoPatchDTO req);
}