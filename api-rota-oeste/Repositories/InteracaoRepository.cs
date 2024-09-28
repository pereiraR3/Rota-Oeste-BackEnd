using api_rota_oeste.Data;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Repositories.Interfaces;

namespace api_rota_oeste.Repositories;

public class InteracaoRepository: IInteracaoRepository {

    private readonly ApiDBContext _context;

    // Construtor para injeção de dependência do contexto
    public InteracaoRepository(ApiDBContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public void criar(InteracaoModel interacaoModel){
        _context.Interacoes.Add(interacaoModel);
        _context.SaveChanges();
    }
}