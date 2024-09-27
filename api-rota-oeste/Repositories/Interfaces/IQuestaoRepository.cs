using api_rota_oeste.Models.Questao;
using api_rota_oeste.Models;

namespace api_rota_oeste.Repositories;
public interface IQuestaoRepository
{
    void criar(QuestaoModel questao);
    
    List<QuestaoModel> listar();
    
    QuestaoModel obter(int id);
    
    void salvar();

    void deletar(int id);
}