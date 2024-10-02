using api_rota_oeste.Models.Interacao;

namespace api_rota_oeste.Services.Interfaces;

public interface IInteracaoService {
    
    public Task<InteracaoResponseDTO> AdicionarAsync(InteracaoRequestDTO interacao);
    
    public Task<InteracaoResponseDTO> BuscarPorIdAsync(int id);

    public Task<List<InteracaoResponseDTO>> BuscarTodosAsync();
    
    public Task<bool> AtualizarAsync(InteracaoPatchDTO interacao);
    
    public Task<bool> ApagarAsync(int id);

    public Task<bool> ApagarTodosAsync();
    
    
}