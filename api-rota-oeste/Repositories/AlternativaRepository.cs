using api_rota_oeste.Data;
using api_rota_oeste.Models.Alternativa;
using api_rota_oeste.Repositories.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Repositories;

public class AlternativaRepository : IAlternativaRepository
{
    
    private readonly ApiDBContext _context;
    private readonly IMapper _mapper;
    
    // Construtor para injeção de dependência do contexto
    public AlternativaRepository(
        
        ApiDBContext context,
        IMapper mapper
        
    )
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /**
     * Método que serve para salvar uma nova instância da entidade alternativa no banco de dados
     */
    public async Task<AlternativaModel> Adicionar(AlternativaModel alternativa)
    {
        
        await _context.AlternativaModels.AddAsync(alternativa);
        
        await _context.SaveChangesAsync();

        return alternativa;
        
    }
    

    /**
     * Método que serve para buscar todas as entidades do tipo alternativa
     */
    public async Task<List<AlternativaModel>> BuscarTodos()
    {
        
        return await _context.AlternativaModels.ToListAsync();
        
    }
    
    /**
     * Método que serve para buscar por determinada entidade do tipo alternativa
     */
    public async Task<AlternativaModel?> BuscarPorId(int id)
    {

        return await _context.AlternativaModels.FindAsync(id);

    }
    
    /**
    * Método que serve para contar o próximo valor para código em relação a criação de uma nova alternativa
    */
    public async Task<int> ObterProximoCodigoPorQuestaoId(int questaoId)
    {
        // Obter o valor máximo de "Codigo" relacionado à "QuestaoId"
        int maxCodigo = await _context.Set<AlternativaModel>()
            .Where(a => a.QuestaoId == questaoId)
            .Select(a => a.Codigo)
            .DefaultIfEmpty(0) // Se não houver registros, retornará 0
            .MaxAsync();

        return maxCodigo + 1; // Incrementar para obter o próximo valor
    }

    
    /**
     * Método que serve para apagar uma entidade do tipo alternativa pelo seu Id
     */
    public async Task<bool> Apagar(int id)
    {
        AlternativaModel? alternativa = await _context.AlternativaModels.FindAsync(id);

        if (alternativa == null)
            return false;
       
        _context.AlternativaModels.Remove(alternativa);
        await _context.SaveChangesAsync();

        return true;
    }
    
}