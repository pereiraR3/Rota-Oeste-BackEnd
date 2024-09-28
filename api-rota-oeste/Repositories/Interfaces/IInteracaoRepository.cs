using api_rota_oeste.Models.Interacao;

namespace api_rota_oeste.Repositories.Interfaces;

public interface IInteracaoRepository {
    
    void criar(InteracaoModel interacao);
    
}