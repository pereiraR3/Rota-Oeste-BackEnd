using api_rota_oeste.Data;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Repositories.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Repositories;

/// <summary>
/// Repositório responsável pelas operações de persistência da entidade Cliente no banco de dados.
/// </summary>
/// <remarks>
/// Implementa a interface <see cref="IClienteRepository"/> e define métodos para realizar operações CRUD
/// e outras operações relacionadas à entidade Cliente.
/// </remarks>
public class ClienteRepository : IClienteRepository
{
    
    private readonly IMapper _mapper;
    private readonly ApiDbContext _dbContext;
    
    public ClienteRepository(IMapper mapper, ApiDbContext dbContext, IUsuarioRepository usuarioRepository)
    {
        _mapper = mapper; // Injetando AutoMapper
        _dbContext = dbContext; // Injetando contexto de DB
    }
    
    /// <summary>
    /// Adiciona uma nova instância da entidade Cliente ao banco de dados.
    /// </summary>
    /// <param name="clienteModel">Objeto contendo os dados do cliente a ser adicionado.</param>
    /// <returns>Retorna o cliente adicionado.</returns>
    public async Task<ClienteModel> Adicionar(ClienteModel clienteModel)
    {
        
        await _dbContext.AddAsync(clienteModel);
        await _dbContext.SaveChangesAsync();
        
        return clienteModel;
    }
    
    /// <summary>
    /// Adiciona uma coleção de instâncias da entidade Cliente ao banco de dados.
    /// </summary>
    /// <param name="clienteModels">Lista de objetos contendo os dados dos clientes a serem adicionados.</param>
    /// <returns>Retorna a lista de clientes adicionados.</returns>
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

    /// <summary>
    /// Busca uma instância da entidade Cliente pelo ID.
    /// </summary>
    /// <param name="id">ID do cliente a ser buscado.</param>
    /// <returns>Retorna o cliente correspondente ao ID fornecido, ou null se não for encontrado.</returns>
    public async Task<ClienteModel?> BuscarPorId(int id)
    {
        
        return await _dbContext.Clientes.FindAsync(id);
        
    }

    /// <summary>
    /// Busca uma instância da entidade Cliente pelo Telefone.
    /// </summary>
    /// <param name="telefone">Telefone do cliente a ser buscado.</param>
    /// <returns>Retorna o cliente correspondente ao Telefone fornecido, ou null se não for encontrado.</returns>
    public async Task<ClienteModel?> BuscarPorTelefone(string telefone)
    {
        return await _dbContext.Clientes.FirstOrDefaultAsync(x => x.Telefone == telefone);
    }

    /// <summary>
    /// Busca todas as instâncias da entidade Cliente armazenadas no banco de dados.
    /// </summary>
    /// <returns>Retorna uma lista de todos os clientes.</returns>
    public async Task<List<ClienteModel>> BuscarTodos()
    {
        
        return await _dbContext.Clientes.ToListAsync();
        
    }

    /// <summary>
    /// Remove uma instância da entidade Cliente pelo ID.
    /// </summary>
    /// <param name="id">ID do cliente a ser removido.</param>
    /// <returns>Retorna true se o cliente for removido com sucesso, caso contrário, retorna false.</returns>
    public async Task<bool> Apagar(int id)
    {
        
        ClienteModel? cliente = await _dbContext.Clientes.FirstOrDefaultAsync(c => c.Id == id);

        if (cliente == null)
            return false;

        _dbContext.Remove(cliente);
        await _dbContext.SaveChangesAsync();
        
        return true;

    }

    /// <summary>
    /// Remove todas as instâncias da entidade Cliente armazenadas no banco de dados.
    /// </summary>
    /// <returns>Retorna true se todos os clientes forem removidos com sucesso, caso contrário, retorna false.</returns>
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