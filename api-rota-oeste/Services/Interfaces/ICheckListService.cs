using api_rota_oeste.Models.CheckList;

namespace api_rota_oeste.Services.Interfaces
{
    public interface ICheckListService
    {
        Task<CheckListResponseDTO> AdicionarAsync(CheckListRequestDTO req);
        
        Task<CheckListResponseDTO?> BuscarPorIdAsync(int id);

        Task<List<CheckListResponseDTO>> BuscarTodosAsync();

        Task<bool> AtualizarAsync(CheckListPatchDTO checkListPatchDto);
        
        Task<bool> ApagarAsync(int id);

        Task<bool> ApagarTodosAsync();
    }
}
