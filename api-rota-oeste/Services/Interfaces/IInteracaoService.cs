using api_rota_oeste.Models.Interacao;

namespace api_rota_oeste.Services.Interfaces;

public interface IInteracaoService {
    
    public Task<InteracaoResponseDTO> AdicionarAsync(InteracaoRequestDTO interacao);
    
    public Task<InteracaoResponseDTO> BuscarPorIdAsync(int id);

    public Task<bool> AtualizarAsync(InteracaoPatchDTO interacao);

}