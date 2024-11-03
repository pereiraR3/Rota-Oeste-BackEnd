using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.ClienteRespondeCheckList;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services.Interfaces;

public interface ICheckListProcessService
{
    Task<ClienteRespondeCheckListResponseDTO> ProcessarCheckListAsync(int clienteId, int checkListId);
    Task<CheckListModel?> BuscarPorIdAsync(int checkListId);
}

public class CheckListProcessService : ICheckListProcessService
{
    private readonly ICheckListService _checkListService;
    private readonly ICheckListRepository _checkListRepository;
    
    public CheckListProcessService(
        ICheckListService checkListService,
        ICheckListRepository checkListRepository
        )
    {
        _checkListService = checkListService;
        _checkListRepository = checkListRepository;
    }

    public async Task<CheckListModel?> BuscarPorIdAsync(int checkListId)
    {
       return await _checkListRepository.BuscarPorId(checkListId);
    }
    
    public async Task<ClienteRespondeCheckListResponseDTO> ProcessarCheckListAsync(int clienteId, int checkListId)
    {
        return await _checkListService.AdicionarClienteRespondeCheckAsync(clienteId, checkListId);
    }
    
    
}