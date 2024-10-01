using api_rota_oeste.Data;
using api_rota_oeste.Repositories.Interfaces;

namespace api_rota_oeste.Repositories;

public class Repository : IRepository
{

    private readonly ApiDBContext _context;
    
    public Repository(
        
        ApiDBContext context
        
        )
    {
        _context = context;
    }
    
    public void Salvar()
    {
        _context.SaveChanges();
    }
}