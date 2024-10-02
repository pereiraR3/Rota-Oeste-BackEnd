using api_rota_oeste.Data;
using api_rota_oeste.Models.ClienteRespondeCheckList;
using api_rota_oeste.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Repositories;

public class ClienteRespondeCheckListRepository : IClienteRespondeCheckListRepository
{

    private readonly ApiDBContext _context;
    
    public ClienteRespondeCheckListRepository(ApiDBContext context)
    {
        _context = context;
    }
    
    public async Task<ClienteRespondeCheckListModel> Adicionar(ClienteRespondeCheckListModel clienteRespondeCheckList)
    {
        
        await _context.AddAsync(clienteRespondeCheckList);
        await _context.SaveChangesAsync();

        // Carregando as entidades de navegação
        await _context.Entry(clienteRespondeCheckList)
            .Reference(crc => crc.Cliente)
            .LoadAsync();
        await _context.Entry(clienteRespondeCheckList)
            .Reference(crc => crc.CheckList)
            .LoadAsync();
        
        return clienteRespondeCheckList;

    }

    public async Task<bool> Apagar(int clienteId, int checkListId)
    {
       ClienteRespondeCheckListModel? clienteResponde = await _context
           .ClienteRespondeCheckListModels
           .FirstOrDefaultAsync(x => x.ClienteId == clienteId && x.CheckListId == checkListId);
       
       if(clienteResponde != null)
        _context.Remove(clienteResponde);
       else
           return false;
       
       await _context.SaveChangesAsync();
        
       return true;
       
    }
}