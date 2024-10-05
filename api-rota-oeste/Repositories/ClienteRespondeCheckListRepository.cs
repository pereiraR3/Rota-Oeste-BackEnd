using api_rota_oeste.Data;
using api_rota_oeste.Models.ClienteRespondeCheckList;
using api_rota_oeste.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Repositories;

public class ClienteRespondeCheckListRepository : IClienteRespondeCheckListRepository
{

    private readonly ApiDbContext _context;
    
    public ClienteRespondeCheckListRepository(ApiDbContext context)
    {
        _context = context;
    }
    
    public async Task<ClienteRespondeCheckListModel?> Adicionar(ClienteRespondeCheckListModel clienteRespondeCheckList)
    {
        // Adicionando e salvando no banco de dados
        await _context.AddAsync(clienteRespondeCheckList);
        await _context.SaveChangesAsync();
    
        // Retornando o objeto adicionado, que já estará atualizado com o ID gerado, sem a necessidade de carregar referências explicitamente
        var resultado = await _context.ClienteRespondeCheckListModels
            .Include(crc => crc.Cliente)
            .Include(crc => crc.CheckList)
            .AsSplitQuery()
            .FirstOrDefaultAsync(crc => crc.ClienteId == clienteRespondeCheckList.ClienteId && crc.CheckListId == clienteRespondeCheckList.CheckListId);

        return resultado;
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