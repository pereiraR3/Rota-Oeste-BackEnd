using api_rota_oeste.Data;
using api_rota_oeste.Models.ClienteRespondeCheckList;
using api_rota_oeste.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Repositories;

/// <summary>
/// Repositório responsável pelas operações de persistência da entidade ClienteRespondeCheckList no banco de dados.
/// </summary>
/// <remarks>
/// Implementa a interface <see cref="IClienteRespondeCheckListRepository"/> e define métodos para realizar operações CRUD
/// relacionadas à entidade ClienteRespondeCheckList, incluindo adição e exclusão de registros.
/// </remarks>
public class ClienteRespondeCheckListRepository : IClienteRespondeCheckListRepository
{

    private readonly ApiDbContext _context;
    
    public ClienteRespondeCheckListRepository(ApiDbContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Adiciona uma nova instância da entidade ClienteRespondeCheckList ao banco de dados.
    /// </summary>
    /// <param name="clienteRespondeCheckList">Objeto contendo os dados da relação Cliente-CheckList a ser adicionada.</param>
    /// <returns>Retorna a relação Cliente-CheckList adicionada, incluindo os dados do Cliente e do CheckList relacionados.</returns>
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

    /// <summary>
    /// Remove uma instância da entidade ClienteRespondeCheckList com base nos IDs do Cliente e do CheckList.
    /// </summary>
    /// <param name="clienteId">ID do cliente associado.</param>
    /// <param name="checkListId">ID do checklist associado.</param>
    /// <returns>Retorna true se a relação Cliente-CheckList for removida com sucesso, caso contrário, retorna false.</returns>
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