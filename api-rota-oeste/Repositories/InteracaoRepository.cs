using api_rota_oeste.Data;
using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Repositories.Interfaces;
using AutoMapper;

namespace api_rota_oeste.Repositories;

/**
 * Representa a camada de persistência de dados, isto é, em relação à classe interacao
 */
public class InteracaoRepository: IInteracaoRepository 
{

    private readonly ApiDBContext _context;
    private readonly IMapper _mapper;
    
    // Construtor para injeção de dependência do contexto
    public InteracaoRepository(
        
        ApiDBContext context,
        IMapper mapper
        )
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper;
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
    
    
}