using api_rota_oeste.Models.Alternativa;

namespace api_rota_oeste.Services.Interfaces;

/**
 * Interface que serve para definir os metódos que irão operar na camada de persistência
 * em relação à entidade Alternativa
 */
public interface IAlternativaService
{
    public Task<AlternativaResponseMinDTO> AdicionarAsync(AlternativaRequestDTO questao);
    
    public Task<AlternativaResponseMinDTO> BuscarPorIdAsync(int id);
    
    public Task<List<AlternativaResponseMinDTO>> BuscarTodosAsync();

    public Task<bool> AtualizarAsync(AlternativaPatchDTO alternativaPatchDto);

    public Task<bool> ApagarAsync(int id);
}