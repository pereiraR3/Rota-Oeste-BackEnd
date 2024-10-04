using api_rota_oeste.Models.Alternativa;
using api_rota_oeste.Models.Questao;

namespace api_rota_oeste.Repositories.Interfaces;

/**
 * Interface que serve para definir os metódos que irão operar na camada de persistência
 * em relação à entidade Alternativa
 */
public interface IAlternativaRepository
{
    Task<AlternativaModel> Adicionar(AlternativaModel alternativa);
    
    Task<AlternativaModel?> BuscarPorId(int id);
    
    Task<List<AlternativaModel>> BuscarTodos();

    Task<int> ObterProximoCodigoPorQuestaoId(int questaoId);
    
    Task<bool> Apagar(int id);
}