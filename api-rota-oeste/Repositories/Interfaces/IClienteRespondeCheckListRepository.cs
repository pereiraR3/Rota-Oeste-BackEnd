using api_rota_oeste.Models.ClienteRespondeCheckList;

namespace api_rota_oeste.Repositories.Interfaces;

public interface IClienteRespondeCheckListRepository
{

    Task<ClienteRespondeCheckListModel?> Adicionar(ClienteRespondeCheckListModel clienteRespondeCheckList);
    
    Task<bool> Apagar(int clienteId, int checkListId);

}