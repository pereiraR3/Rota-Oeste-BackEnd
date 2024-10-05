using api_rota_oeste.Models.CheckList;

/**
 * Interface que serve para definir os metódos que irão operar na camada de persistência
 * em relação à entidade CheckList
 */
namespace api_rota_oeste.Repositories.Interfaces
{
    public interface ICheckListRepository
    {
        Task<CheckListModel?> Adicionar(CheckListModel checkListModel);
        
        Task<CheckListModel?> BuscarPorId(int id);

        Task<List<CheckListModel>> BuscarTodos();

        Task<bool> Apagar(int id);

        Task<bool> ApagarTodos();

        public Task<IEnumerable<dynamic>> GerarRelatorioGeral(int idChecklist);
    }
}
