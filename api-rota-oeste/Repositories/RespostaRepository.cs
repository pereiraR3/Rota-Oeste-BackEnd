using api_rota_oeste.Data;
using api_rota_oeste.Models.RespostaAlternativa;
using api_rota_oeste.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Repositories;

public class RespostaRepository : IRespostaRepository

{
    
    private readonly ApiDBContext _dbContext;
    
    // Construtor para injeção de dependência do contexto
    public RespostaRepository(
        
        ApiDBContext context
        
        )
    {
        _dbContext = context ?? throw new ArgumentNullException(nameof(context));

    }
    
    /**
     * Método que serve para salvar uma nova instância da entidade usuario no banco de dados
     */
    public async Task<RespostaModel?> Adicionar(RespostaModel resposta)
    {
        
        await _dbContext.RespostaModels.AddAsync(resposta);
        
        await _dbContext.SaveChangesAsync();

        return resposta;
    }
    

    /**
     * Método que serve para recuperar determinado usuario por meio de seu ID
     */
    public async Task<RespostaModel?> BuscaPorId(int id)
    {

        return await _dbContext.RespostaModels.FindAsync(id);

    }

    /**
     * Método usado para realizar deleção relacional de uma entidade do tipo Usuario
     */
    public async Task<bool> Apagar(int id)
    {
        RespostaModel? respostaAlternativa = await _dbContext.RespostaModels.FindAsync(id);

        if (respostaAlternativa == null)
            return false;
       
        _dbContext.RespostaModels.Remove(respostaAlternativa);
        await _dbContext.SaveChangesAsync();

        return true;
    }
    
    
    /**
     * Método usado para apagar a todos as entidades do tipo RespostaAlternativaModel
     */
    public async Task<bool> ApagarTodos()
    {
       
        _dbContext.RespostaModels.RemoveRange();
        
        await _dbContext.SaveChangesAsync();

        return true;
    }
    
}