using api_rota_oeste.Models.Interacao;

namespace api_rota_oeste.Services.Interfaces;

public interface IInteracaoService {
    
    public void criar(InteracaoRequestDTO interacaoModel);

    public Task<InteracaoResponseDTO> CriarAsync(InteracaoRequestDTO req);

    public Task<InteracaoModel?> BuscarPorId(int id);

    public Task<bool> Atualizar(InteracaoPatchDTO req);
}