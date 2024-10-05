using api_rota_oeste.Models.RespostaAlternativa;
using api_rota_oeste.Models.RespostaTemAlternativa;

namespace api_rota_oeste.Services.Interfaces;

public interface IRespostaService
{
    
    Task<RespostaResponseDTO> AdicionarAsync(RespostaRequestDTO resposta);

    Task<RespostaTemAlternativaResponseDTO> AdicionarRespostaTemAlternativaAsync(int respostaId, int alternativaId);
    
    Task<RespostaResponseDTO> BuscarPorIdAsync(int id);
    
    Task<bool> AtualizarAsync(RespostaPatchDTO resposta);    
    
    Task<bool> ApagarAsync(int id);
    
    Task<bool> ApagarRespostaTemAlternativaAsync(int respostaId, int alternativaId);
    
    Task<bool> ApagarTodosAsync();
    
}