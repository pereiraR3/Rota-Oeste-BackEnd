using api_rota_oeste.Data;
using api_rota_oeste.Repositories.Interfaces;

namespace api_rota_oeste.Repositories;

public class Repository : IRepository
{

    private readonly ApiDbContext _context;
    
    public Repository(
        
        ApiDbContext context
        
        )
    {
        _context = context;
    }
    
    public void Salvar()
    {
        _context.SaveChanges();
    }
}