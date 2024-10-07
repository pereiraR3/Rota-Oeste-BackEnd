using api_rota_oeste.Data;
using api_rota_oeste.Models.RespostaAlternativa;
using api_rota_oeste.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Repositories;

/// <summary>
/// Repositório responsável pelas operações de persistência da entidade Resposta no banco de dados.
/// </summary>
/// <remarks>
/// Implementa a interface <see cref="IRespostaRepository"/> e define métodos para realizar operações CRUD
/// relacionadas à entidade Resposta.
/// </remarks>
public class RespostaRepository : IRespostaRepository

{
    
    private readonly ApiDbContext _dbContext;
    
    // Construtor para injeção de dependência do contexto
    public RespostaRepository(
        
        ApiDbContext context
        
        )
    {
        _dbContext = context ?? throw new ArgumentNullException(nameof(context));

    }
    
    /// <summary>
    /// Salva uma nova instância da entidade Resposta no banco de dados.
    /// </summary>
    /// <param name="resposta">Objeto contendo os dados da resposta a ser adicionada.</param>
    /// <returns>Retorna a resposta adicionada.</returns>
    public async Task<RespostaModel?> Adicionar(RespostaModel resposta)
    {
        
        await _dbContext.RespostaModels.AddAsync(resposta);
        
        await _dbContext.SaveChangesAsync();

        return resposta;
    }
    

    /// <summary>
    /// Busca uma instância da entidade Resposta pelo ID.
    /// </summary>
    /// <param name="id">ID da resposta a ser buscada.</param>
    /// <returns>Retorna a resposta correspondente ao ID fornecido, ou null se não for encontrada.</returns>
    public async Task<RespostaModel?> BuscaPorId(int id)
    {

        return await _dbContext.RespostaModels.FindAsync(id);

    }

    /// <summary>
    /// Remove uma instância da entidade Resposta pelo ID.
    /// </summary>
    /// <param name="id">ID da resposta a ser removida.</param>
    /// <returns>Retorna true se a resposta for removida com sucesso, caso contrário, retorna false.</returns>
    public async Task<bool> Apagar(int id)
    {
        RespostaModel? respostaAlternativa = await _dbContext.RespostaModels.FindAsync(id);

        if (respostaAlternativa == null)
            return false;
       
        _dbContext.RespostaModels.Remove(respostaAlternativa);
        await _dbContext.SaveChangesAsync();

        return true;
    }
    
    /// <summary>
    /// Remove todas as instâncias da entidade Resposta armazenadas no banco de dados.
    /// </summary>
    /// <returns>Retorna true após remover todas as respostas com sucesso.</returns>
    public async Task<bool> ApagarTodos()
    {
       
        _dbContext.RespostaModels.RemoveRange();
        
        await _dbContext.SaveChangesAsync();

        return true;
    }
    
}