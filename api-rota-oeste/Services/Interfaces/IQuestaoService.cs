using api_rota_oeste.Models.Questao;

namespace api_rota_oeste.Services.Interfaces;

public interface IQuestaoService {

    public Task<QuestaoResponseDTO> AdicionarAsync(QuestaoRequestDTO questao);
    
    public Task<QuestaoResponseDTO> BuscarPorIdAsync(int id);
    
    public Task<List<QuestaoResponseDTO>> BuscarTodosAsync();

    public Task<bool> AtualizarAsync(QuestaoPatchDTO request);

    public Task<bool> ApagarAsync(int id);
    
}