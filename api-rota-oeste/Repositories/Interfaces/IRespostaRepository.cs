using api_rota_oeste.Models.RespostaAlternativa;

namespace api_rota_oeste.Services.Interfaces;

/**
 * Interface que serve para definir os metódos que irão operar na camada de persistência
 * em relação à entidade RespostaAlternativa
 */
public interface IRespostaRepository
{
    
    Task<RespostaModel?> Adicionar(RespostaModel respostaAlternativa);

    Task<RespostaModel?> BuscaPorId(int id);
    
    Task<bool> Apagar(int id);
    
    Task<bool> ApagarTodos();
    
}