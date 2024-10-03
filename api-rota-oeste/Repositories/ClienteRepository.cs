using api_rota_oeste.Data;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Repositories.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Repositories;

public class ClienteRepository : IClienteRepository
{
    
    private readonly IMapper _mapper;
    private readonly ApiDBContext _dbContext;
    
    public ClienteRepository(IMapper mapper, ApiDBContext dbContext, IUsuarioRepository usuarioRepository)
    {
        _mapper = mapper; // Injetando AutoMapper
        _dbContext = dbContext; // Injetando contexto de DB
    }
    
    public async Task<ClienteModel> Adicionar(ClienteModel clienteModel)
    {
        
        await _dbContext.AddAsync(clienteModel);
        await _dbContext.SaveChangesAsync();
        
        return clienteModel;
    }
    

    /**
     * Inserção em massa de entidades do tipo cliente
     */
    public async Task<List<ClienteModel>> AdicionarColecao(List<ClienteModel> clienteModels)
    {
        
        List<ClienteModel> clientes = new List<ClienteModel>();
        
        foreach (var cliente in clienteModels)
        {
            
            _dbContext.Add(cliente);
            await _dbContext.SaveChangesAsync();
            
            clientes.Add(cliente);
            
        }
        
        return clientes.ToList();
        
    }

    /**
     * Método que serve para recuperar determinado usuario por meio de seu ID
     */
    public async Task<ClienteModel?> BuscarPorId(int id)
    {
        
        return await _dbContext.Clientes.FindAsync(id);
        
    }

    /**
     * Método que serve para recuperar todos os clientes do banco de dados
     */
    public async Task<List<ClienteModel>> BuscarTodos()
    {
        
        return await _dbContext.Clientes.ToListAsync();
        
    }

    /**
     * Método usado para realizar deleção relacional de uma entidade do tipo Usuario
     */
    public async Task<bool> Apagar(int id)
    {
        
        ClienteModel? cliente = await _dbContext.Clientes.FirstOrDefaultAsync(c => c.Id == id);

        if (cliente == null)
            return false;

        _dbContext.Remove(cliente);
        await _dbContext.SaveChangesAsync();
        
        return true;

    }

    /**
     * Método usado para realizar deleção relacional de uma entidade do tipo Usuario
     */
    public async Task<bool> ApagarTodos()
    {
        List<ClienteModel> clientes = await BuscarTodos();

        if (clientes.Count == 0)
            return false;

        foreach (var cliente in clientes)
        {
            if (cliente != null) _dbContext.Clientes.Remove(cliente);
            await _dbContext.SaveChangesAsync();
        }
        
        return true;
        
    }
    
}