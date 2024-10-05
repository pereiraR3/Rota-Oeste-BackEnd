using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.ClienteRespondeCheckList;

namespace api_rota_oeste.Services.Interfaces
{
    public interface ICheckListService
    {
        Task<CheckListResponseDTO?> AdicionarAsync(CheckListRequestDTO req);
        
        Task<ClienteRespondeCheckListResponseDTO> AdicionarClienteRespondeCheckAsync(int clienteId, int checkListId);
        
        Task<bool> ApagarClienteRespondeCheckAsync(int clienteId, int checkListId);
        
        Task<CheckListResponseDTO?> BuscarPorIdAsync(int id);

        Task<List<CheckListResponseDTO>> BuscarTodosAsync();

        Task<bool> AtualizarAsync(CheckListPatchDTO checkListPatchDto);
        
        Task<bool> ApagarAsync(int id);

        Task<bool> ApagarTodosAsync();

        Task<List<CheckListRelatorioGeralDTO>> GerarRelatorioGeralAsync(int idChecklist);
    }
}
