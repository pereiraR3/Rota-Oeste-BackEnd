using api_rota_oeste.Data;
using api_rota_oeste.Repositories.Interfaces;

namespace api_rota_oeste.Repositories;

/// <summary>
/// Repositório genérico responsável por gerenciar operações comuns de persistência no banco de dados.
/// </summary>
/// <remarks>
/// Implementa a interface <see cref="IRepository"/> e fornece métodos para gerenciar transações comuns,
/// como salvar alterações no banco de dados.
/// Esta classe pode ser utilizada por outros repositórios específicos para operações básicas de persistência.
/// </remarks>
public class Repository : IRepository
{

    private readonly ApiDbContext _context;
    
    public Repository(
        
        ApiDbContext context
        
        )
    {
        _context = context;
    }
    
    /// <summary>
    /// Salva todas as alterações feitas no contexto do banco de dados.
    /// </summary>
    /// <remarks>
    /// Este método é usado para persistir quaisquer alterações pendentes no banco de dados,
    /// garantindo que as operações realizadas sejam aplicadas permanentemente.
    /// </remarks>
    public void Salvar()
    {
        _context.SaveChanges();
    }
}