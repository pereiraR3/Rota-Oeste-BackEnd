using api_rota_oeste.Data;
using api_rota_oeste.Models.Usuario;
using api_rota_oeste.Repositories.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Repositories;

/**
 * Representa a camada de persistência de dados, isto é, em relação à classe Usuario
 */
public class UsuarioRepository : IUsuarioRepository
{

    private readonly ApiDBContext _dbContext; // Injetando contexto de DB
    private readonly IMapper _mapper; // Injetando AutoMapper

    public UsuarioRepository(ApiDBContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    /**
     * Método que serve para salvar uma nova instância da entidade usuario no banco de dados
     */
    public async Task<UsuarioModel?> Adicionar(UsuarioModel usuario)
    {
        await _dbContext.Usuarios.AddAsync(usuario);
        await _dbContext.SaveChangesAsync();

        return usuario;
    }

    /**
     * Método que serve para recuperar determinado usuario por meio de seu ID
     */
    public async Task<UsuarioModel?> BuscaPorId(int id)
    {

        UsuarioModel? usuario = await _dbContext
            .Usuarios
            .Include(x => x.Clientes)
            .Include(x => x.CheckLists)
            .FirstOrDefaultAsync(x => x.Id == id);
        
        return usuario;
    }

    /**
     * Método serve para alterar os dados de um usuário parcialmente
     */
    public async Task<bool> Atualizar(UsuarioPatchDTO request)
    {
        UsuarioModel? usuarioModel = await BuscaPorId(request.Id);
        
        if(usuarioModel == null)
            return false;
        
        // O mapeamento de atualização deve ignorar campos nulos
        _mapper.Map(request, usuarioModel);
        
        _dbContext.Usuarios.Update(usuarioModel);
        await _dbContext.SaveChangesAsync();

        return true;

    }

    /**
     * Método usado para realizar deleção relacional de uma entidade do tipo Usuario 
     */
    public async Task<bool> Apagar(int id)
    {
       UsuarioModel? usuario = await _dbContext.Usuarios.FindAsync(id);

       if (usuario == null)
           return false;
       
       _dbContext.Usuarios.Remove(usuario);
       await _dbContext.SaveChangesAsync();

       return true;
    }
    
    
}