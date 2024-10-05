using api_rota_oeste.Data;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Repositories.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Repositories;

/**
 * Representa a camada de persistência de dados, isto é, em relação à classe interacao
 */
public class InteracaoRepository: IInteracaoRepository 
{

    private readonly ApiDbContext _context;
    
    // Construtor para injeção de dependência do contexto
    public InteracaoRepository(
        
        ApiDbContext context,
        IMapper mapper
        )
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    /**
     * Método que serve para salvar uma nova instância da entidade interacao no banco de dados
     */
    public async Task<InteracaoModel?> Adicionar(InteracaoModel interacaoModel)
    {
        
        await _context.Interacoes.AddAsync(interacaoModel);
        
        await _context.SaveChangesAsync();
        
        return interacaoModel;
        
    }

    /**
    * Método que serve para buscar por determinada entidade do tipo interacao
    */
    public async Task<InteracaoModel?> BuscarPorId(int id)
    {
        return await _context.Interacoes.FindAsync(id);
    }

    /**
    * Método que serve para buscar por determinada entidade do tipo interacao
    */
    public async Task<List<InteracaoModel>> BuscarTodos()
    {
        
        return await _context.Interacoes.ToListAsync();
        
    }

    /**
    * Método que serve para buscar por determinada entidade do tipo interacao
    */
    public async Task<bool> ApagarPorId(int id)
    {

        var interacaoModel = await _context.Interacoes.FindAsync(id);
        
        if(interacaoModel == null)
            return false;
        
        _context.Interacoes.Remove(interacaoModel);
        
        await _context.SaveChangesAsync();

        return true;
    }

    /**
    * Método que serve para buscar por determinada entidade do tipo interacao
    */
    public async Task<bool> ApagarTodos()
    {
        
        _context.Interacoes.RemoveRange(_context.Interacoes);
        
        await _context.SaveChangesAsync();

        return true;
        
    }
    
}