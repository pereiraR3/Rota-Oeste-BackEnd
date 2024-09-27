using api_rota_oeste.Models.CheckList;

namespace api_rota_oeste.Services.Interfaces
{
    public interface ICheckListService
    {
        Task<CheckListResponseDTO> AddAsync(CheckListRequestDTO req);

        Task<List<CheckListResponseDTO>> AddCollectionAsync(CheckListCollectionDTO req);

        Task<CheckListResponseDTO?> FindByIdAsync(int id);

        Task<List<CheckListResponseDTO>> GetAllAsync();

        Task<bool> DeleteAsync(int id);

        Task<bool> DeleteAllAsync();
    }
}
