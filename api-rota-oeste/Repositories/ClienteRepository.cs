using api_rota_oeste.Data;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.Usuario;
using api_rota_oeste.Repositories.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Repositories;

public class ClienteRepository : IClienteRepository
{
    
    private readonly IMapper _mapper;
    private readonly ApiDBContext _dbContext;
    private readonly UsuarioRepository _usuarioRepository;
    
    public ClienteRepository(IMapper mapper, ApiDBContext dbContext, UsuarioRepository usuarioRepository)
    {
        _mapper = mapper; // Injetando AutoMapper
        _dbContext = dbContext; // Injetando contexto de DB
        _usuarioRepository = usuarioRepository;
    }
    
    public async Task<ClienteModel> Adicionar(ClienteRequestDTO request)
    {
        
        UsuarioModel usuario = await _usuarioRepository.BuscaPorId(request.UsuarioId);
        
        if (usuario == null)
            throw new Exception($"Usuário não encontrado ID: {request.UsuarioId}");
        
        ClienteModel cliente = new ClienteModel(request, usuario);
        
        await _dbContext.AddAsync(cliente);
        await _dbContext.SaveChangesAsync();
        
        return cliente;
    }

    public async Task<List<ClienteModel>> AdicionarColecao(ClienteCollectionDTO request)
    {
        throw new NotImplementedException();
    }

    /**
     * Método que serve para recuperar determinado usuario por meio de seu ID
     */
    public async Task<ClienteModel> BuscaPorId(int id)
    {
        try
        {
            ClienteModel cliente = _dbContext.Clientes.FirstOrDefault(c => c.Id == id);

            return cliente;
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao buscar cliente: {ex.Message}");
            return null;
        }
    }

    /**
     * Método que serve para recuperar todos os clientes do banco de dados
     */
    public async Task<List<ClienteModel>> BuscaTodos()
    {
        List<ClienteModel> clientes = await _dbContext.Clientes.ToListAsync();
        
        return clientes;
        
    }

    /**
     * Método usado para realizar deleção relacional de uma entidade do tipo Usuario
     */
    public async Task<bool> Apagar(int id)
    {
        
        ClienteModel cliente = await _dbContext.Clientes.FirstOrDefaultAsync(c => c.Id == id);

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
        return false;
    }
    
}