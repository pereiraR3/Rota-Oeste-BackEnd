using api_rota_oeste.Models.RespostaTemAlternativa;

namespace api_rota_oeste.Repositories.Interfaces;

public interface IRespostaTemAlternativaRepository
{
    
    Task<RespostaTemAlternativaModel?> Adicionar(RespostaTemAlternativaModel respostaTemAlternativaModel);
    
    Task<bool> Apagar(int respostaId, int alternativaId);
    
}