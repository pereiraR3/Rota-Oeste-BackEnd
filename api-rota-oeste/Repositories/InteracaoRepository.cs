using api_rota_oeste.Data;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Repositories.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Repositories;

/// <summary>
/// Repositório responsável pelas operações de persistência da entidade Interacao no banco de dados.
/// </summary>
/// <remarks>
/// Implementa a interface <see cref="IInteracaoRepository"/> e define métodos para realizar operações CRUD
/// relacionadas à entidade Interacao.
/// </remarks>
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
    
    /// <summary>
    /// Salva uma nova instância da entidade Interacao no banco de dados.
    /// </summary>
    /// <param name="interacaoModel">Objeto contendo os dados da interação a ser adicionada.</param>
    /// <returns>Retorna a interação adicionada.</returns>
    public async Task<InteracaoModel?> Adicionar(InteracaoModel interacaoModel)
    {
        
        await _context.Interacoes.AddAsync(interacaoModel);
        
        await _context.SaveChangesAsync();
        
        return interacaoModel;
        
    }

    /// <summary>
    /// Busca uma instância da entidade Interacao pelo ID.
    /// </summary>
    /// <param name="id">ID da interação a ser buscada.</param>
    /// <returns>Retorna a interação correspondente ao ID fornecido, ou null se não for encontrada.</returns>
    public async Task<InteracaoModel?> BuscarPorId(int id)
    {
        return await _context.Interacoes.FindAsync(id);
    }
    
    public async Task<InteracaoModel?> BuscarPorIdCliente(int clienteExistenteId)
    {
        return await _context.Interacoes
            .Where(c => c.ClienteId == clienteExistenteId)
            .OrderByDescending(c => c.Data)
            .Include(c => c.RespostaAlternativaModels)
            .Include(c => c.CheckList)
            .ThenInclude(cl => cl.Questoes)
            .ThenInclude(cl => cl.AlternativaModels)
            .FirstOrDefaultAsync();

    }

    /// <summary>
    /// Busca todas as instâncias da entidade Interacao armazenadas no banco de dados.
    /// </summary>
    /// <returns>Retorna uma lista de todas as interações.</returns>
    public async Task<List<InteracaoModel>> BuscarTodos()
    {
        
        return await _context.Interacoes.ToListAsync();
        
    }

    /// <summary>
    /// Remove uma instância da entidade Interacao pelo ID.
    /// </summary>
    /// <param name="id">ID da interação a ser removida.</param>
    /// <returns>Retorna true se a interação for removida com sucesso, caso contrário, retorna false.</returns>
    public async Task<bool> ApagarPorId(int id)
    {

        var interacaoModel = await _context.Interacoes.FindAsync(id);
        
        if(interacaoModel == null)
            return false;
        
        _context.Interacoes.Remove(interacaoModel);
        
        await _context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Remove todas as instâncias da entidade Interacao armazenadas no banco de dados.
    /// </summary>
    /// <returns>Retorna true após remover todas as interações com sucesso.</returns>
    public async Task<bool> ApagarTodos()
    {
        
        _context.Interacoes.RemoveRange(_context.Interacoes);
        
        await _context.SaveChangesAsync();

        return true;
        
    }
}