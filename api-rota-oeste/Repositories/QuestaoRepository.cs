using api_rota_oeste.Data;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.Questao;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Repositories;

/// <summary>
/// Repositório responsável pelas operações de persistência da entidade Questao no banco de dados.
/// </summary>
/// <remarks>
/// Implementa a interface <see cref="IQuestaoRepository"/> e define métodos para realizar operações CRUD
/// relacionadas à entidade Questao.
/// </remarks>
public class QuestaoRepository : IQuestaoRepository
{
    private readonly ApiDbContext _context;
    private readonly IMapper _mapper;
    
    // Construtor para injeção de dependência do contexto
    public QuestaoRepository(
        
        ApiDbContext context,
        IMapper mapper
        
    )
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Salva uma nova instância da entidade Questao no banco de dados.
    /// </summary>
    /// <param name="questao">Objeto contendo os dados da questão a ser adicionada.</param>
    /// <returns>Retorna a questão adicionada.</returns>
    public async Task<QuestaoModel> Adicionar(QuestaoModel questao)
    {
        
        await _context.Questoes.AddAsync(questao);
        
        await _context.SaveChangesAsync();

        return questao;
        
    }
    
    /// <summary>
    /// Busca todas as instâncias da entidade Questao armazenadas no banco de dados.
    /// </summary>
    /// <returns>Retorna uma lista de todas as questões.</returns>
    public async Task<List<QuestaoModel>> BuscarTodos()
    {
        return await _context.Questoes.ToListAsync();
        
    }

    /// <summary>
    /// Busca uma instância da entidade Questao pelo ID.
    /// </summary>
    /// <param name="id">ID da questão a ser buscada.</param>
    /// <returns>Retorna a questão correspondente ao ID fornecido, ou null se não for encontrada.</returns>
    public async Task<QuestaoModel?> BuscarPorId(int id)
    {

        return await _context
            .Questoes
            .Include(x => x.AlternativaModels)
            .FirstOrDefaultAsync(x => x.Id == id);

    }
    
    /// <summary>
    /// Remove uma instância da entidade Questao pelo ID.
    /// </summary>
    /// <param name="id">ID da questão a ser removida.</param>
    /// <returns>Retorna true se a questão for removida com sucesso, caso contrário, retorna false.</returns>
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