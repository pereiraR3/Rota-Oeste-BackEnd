using api_rota_oeste.Models.Questao;

namespace api_rota_oeste.Repositories;

/**
 * Interface que serve para definir os metódos que irão operar na camada de persistência
 * em relação à entidade Questao
 */
public interface IQuestaoRepository
{
    Task<QuestaoModel> Adicionar(QuestaoModel questao);
    
    Task<List<QuestaoModel>> BuscarTodos();
    
    Task<QuestaoModel?> BuscarPorId(int id);
    
    Task<bool> Apagar(int id);
}