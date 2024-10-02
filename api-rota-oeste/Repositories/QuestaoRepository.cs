using api_rota_oeste.Data;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.Questao;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Repositories;

/**
 * Representa a camada de persistência de dados, isto é, em relação à classe Usuario
 */
public class QuestaoRepository : IQuestaoRepository
{
    private readonly ApiDBContext _context;
    private readonly IMapper _mapper;
    
    // Construtor para injeção de dependência do contexto
    public QuestaoRepository(
        
        ApiDBContext context,
        IMapper mapper
        
    )
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /**
     * Método que serve para salvar uma nova instância da entidade questao no banco de dados
     */
    public async Task<QuestaoModel> Adicionar(QuestaoModel questao)
    {
        
        await _context.Questoes.AddAsync(questao);
        
        await _context.SaveChangesAsync();

        return questao;
        
    }
    

    /**
     * Método que serve para buscar todas as entidades do tipo questao
     */
    public async Task<List<QuestaoModel>> BuscarTodos()
    {
        return await _context.Questoes.ToListAsync();
        
    }

    /**
     * Método que serve para buscar por determinada entidade do tipo questao 
     */
    public async Task<QuestaoModel?> BuscarPorId(int id)
    {
        
        return await _context.Questoes
            .Include(x => x.CheckList)
            .Include(x => x.RespostaAlternativaModels)
            .FirstOrDefaultAsync(x => x.Id == id);
        
    }
    
    /**
     * Método que serve para apagar uma entidade do tipo questao pelo seu Id
     */
    public async Task<bool> Apagar(int id)
    {
        QuestaoModel? questao = await _context.Questoes.FindAsync(id);

        if (questao == null)
            return false;
       
        _context.Questoes.Remove(questao);
        await _context.SaveChangesAsync();

        return true;
    }
    
}