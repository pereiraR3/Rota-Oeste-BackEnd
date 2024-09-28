using api_rota_oeste.Models.Questao;

namespace api_rota_oeste.Services.Interfaces;

public interface IQuestaoService {

    public void criar(QuestaoRequestDTO questao);
    
    public List<QuestaoModel> listar();

    public QuestaoResponseDTO obter(int id);

    public void editar(int id, QuestaoRequestDTO editar);

    public void deletar(int id);
}