using api_rota_oeste.Models.Interacao;

namespace api_rota_oeste.Repositories.Interfaces;

public interface IInteracaoRepository {
    
    Task<InteracaoModel?> Adicionar(InteracaoModel interacaoModel);
    
    Task<InteracaoModel?> BuscarPorId(int id);
    
}