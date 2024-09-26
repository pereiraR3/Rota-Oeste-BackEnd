using api_rota_oeste.Models.Questao;
using api_rota_oeste.Models;

namespace PrimeiraAPI.Repository;

public interface IQuestaoRepository
{
    void criar(QuestaoModel questao);
    
    List<QuestaoModel> listar();
    
    QuestaoModel obter(int id);
    
    void salvar();

    void deletar(int id);
}