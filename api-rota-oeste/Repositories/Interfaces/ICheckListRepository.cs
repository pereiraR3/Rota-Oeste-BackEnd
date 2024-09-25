using api_rota_oeste.Models.CheckList;

namespace api_rota_oeste.Repositories.Interfaces
{
    public interface ICheckListRepository
    {
        Task<CheckListModel> Add(CheckListRequestDTO req);

        Task<List<CheckListModel>> AddCollection(CheckListCollectionDTO req);

        Task<CheckListModel?> FindById(int id);

        Task<List<CheckListModel?>> GetAll();

        Task<bool> Delete(int id);

        Task<bool> DeleteAll();
    }
}
