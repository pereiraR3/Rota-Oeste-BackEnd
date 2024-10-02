using api_rota_oeste.Models.RespostaAlternativa;

namespace api_rota_oeste.Services.Interfaces;

public interface IRespostaAlternativaService
{
    
    Task<RespostaAlternativaResponseDTO> AdicionarAsync(RespostaAlternativaRequestDTO respostaAlternativa);

    Task<RespostaAlternativaResponseDTO> BuscarPorIdAsync(int id);
    
    Task<bool> AtualizarAsync(RespostaAlternativaPatchDTO respostaAlternativa);    
    
    Task<bool> ApagarAsync(int id);
    
    Task<bool> ApagarTodosAsync();
    
}