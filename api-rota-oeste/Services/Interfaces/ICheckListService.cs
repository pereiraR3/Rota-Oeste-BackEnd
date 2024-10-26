using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.ClienteRespondeCheckList;

namespace api_rota_oeste.Services.Interfaces
{
    public interface ICheckListService
    {
        Task<CheckListResponseMinDTO?> AdicionarAsync(CheckListRequestDTO req);

        Task<CheckListResponseMinDTO?> AdicionarCollectionAsync(CheckListRequestCollection checkListCollectionDto);
        
        Task<ClienteRespondeCheckListResponseDTO> AdicionarClienteRespondeCheckAsync(int clienteId, int checkListId);
        
        Task<CheckListResponseMinDTO?> BuscarPorIdAsync(int id);

        Task<List<CheckListResponseMinDTO>> BuscarTodosAsync();

        Task<bool> AtualizarAsync(CheckListPatchDTO checkListPatchDto);
        
        Task<bool> ApagarAsync(int id);
        
        Task<bool> ApagarClienteRespondeCheckAsync(int clienteId, int checkListId);

        Task<bool> ApagarTodosAsync();
        
        Task<List<CheckListRelatorioGeralDTO>> GerarRelatorioGeralAsync(int idChecklist);
    }
}
