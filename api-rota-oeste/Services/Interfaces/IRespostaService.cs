using api_rota_oeste.Models.RespostaAlternativa;

namespace api_rota_oeste.Services.Interfaces;

public interface IRespostaService
{
    
    Task<RespostaResponseDTO> AdicionarAsync(RespostaRequestDTO resposta);

    Task<RespostaResponseDTO> BuscarPorIdAsync(int id);
    
    Task<bool> AtualizarAsync(RespostaPatchDTO resposta);    
    
    Task<bool> ApagarAsync(int id);
    
    Task<bool> ApagarTodosAsync();
    
}