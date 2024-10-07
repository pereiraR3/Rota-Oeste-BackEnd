using api_rota_oeste.Data;
using api_rota_oeste.Models.Alternativa;
using api_rota_oeste.Repositories.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Repositories;

/// <summary>
/// Repositório responsável pelas operações de persistência da entidade Alternativa no banco de dados.
/// </summary>
/// <remarks>
/// Implementa a interface <see cref="IAlternativaRepository"/> e define métodos para realizar operações CRUD
/// e outras operações relacionadas à entidade Alternativa.
/// </remarks>
public class AlternativaRepository : IAlternativaRepository
{
    
    private readonly ApiDbContext _context;
    private readonly IMapper _mapper;
    
    // Construtor para injeção de dependência do contexto
    public AlternativaRepository(
        
        ApiDbContext context,
        IMapper mapper
        
    )
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Salva uma nova instância da entidade Alternativa no banco de dados.
    /// </summary>
    /// <param name="alternativa">Objeto contendo os dados da alternativa a ser adicionada.</param>
    /// <returns>Retorna a alternativa adicionada.</returns>
    public async Task<AlternativaModel> Adicionar(AlternativaModel alternativa)
    {
        
        await _context.AlternativaModels.AddAsync(alternativa);
        
        await _context.SaveChangesAsync();

        return alternativa;
        
    }
    

    /// <summary>
    /// Busca todas as instâncias da entidade Alternativa armazenadas no banco de dados.
    /// </summary>
    /// <returns>Retorna uma lista de todas as alternativas, incluindo suas respectivas questões associadas.</returns>
    public async Task<List<AlternativaModel>> BuscarTodos()
    {
        
        return await _context.AlternativaModels
            .Include(x => x.Questao)
            .AsNoTracking()
            .ToListAsync();
        
    }
    
    /// <summary>
    /// Busca uma instância da entidade Alternativa pelo ID.
    /// </summary>
    /// <param name="id">ID da alternativa a ser buscada.</param>
    /// <returns>Retorna a alternativa correspondente ao ID fornecido, incluindo a questão associada, ou null se não for encontrada.</returns>
    public async Task<AlternativaModel?> BuscarPorId(int id)
    {
        var alternativa = await _context.AlternativaModels
            .Include(a => a.Questao)
            .FirstOrDefaultAsync(a => a.Id == id);
        return alternativa;
    }
    
    /// <summary>
    /// Obtém o próximo valor de código disponível para uma alternativa associada a uma determinada questão.
    /// </summary>
    /// <param name="questaoId">ID da questão à qual a alternativa está associada.</param>
    /// <returns>Retorna o próximo valor de código disponível para a alternativa da questão especificada.</returns>
    public async Task<int> ObterProximoCodigoPorQuestaoId(int questaoId)
    {
        // Obter o valor máximo de "Codigo" relacionado à "QuestaoId" ou 0 se não houver registros
        int maxCodigo = await _context.AlternativaModels
            .Where(a => a.QuestaoId == questaoId)
            .Select(a => (int?)a.Codigo)
            .MaxAsync() ?? 0;

        return maxCodigo + 1; // Incrementar para obter o próximo valor
    }
    
    /// <summary>
    /// Remove uma instância da entidade Alternativa pelo ID.
    /// </summary>
    /// <param name="id">ID da alternativa a ser removida.</param>
    /// <returns>Retorna true se a alternativa for removida com sucesso, caso contrário, retorna false.</returns>
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