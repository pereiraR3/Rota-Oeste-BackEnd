using api_rota_oeste.Data;
using api_rota_oeste.Models.RespostaAlternativa;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services.Interfaces;

namespace api_rota_oeste.Services;

public class RespostaAlternativaRepository : IRespostaAlternativaRepository

{
    
    private readonly ApiDBContext _dbContext;
    
    // Construtor para injeção de dependência do contexto
    public RespostaAlternativaRepository(
        
        ApiDBContext context
        
        )
    {
        _dbContext = context ?? throw new ArgumentNullException(nameof(context));

    }
    
    /**
     * Método que serve para salvar uma nova instância da entidade usuario no banco de dados
     */
    public async Task<RespostaAlternativaModel?> Adicionar(RespostaAlternativaModel respostaAlternativa)
    {
        
        await _dbContext.RespostaAlternativaModels.AddAsync(respostaAlternativa);
        
        await _dbContext.SaveChangesAsync();

        return respostaAlternativa;
    }
    

    /**
     * Método que serve para recuperar determinado usuario por meio de seu ID
     */
    public async Task<RespostaAlternativaModel?> BuscaPorId(int id)
    {
        
        return await _dbContext.RespostaAlternativaModels.FindAsync(id);
        
    }

    /**
     * Método usado para realizar deleção relacional de uma entidade do tipo Usuario
     */
    public async Task<bool> Apagar(int id)
    {
        RespostaAlternativaModel? respostaAlternativa = await _dbContext.RespostaAlternativaModels.FindAsync(id);

        if (respostaAlternativa == null)
            return false;
       
        _dbContext.RespostaAlternativaModels.Remove(respostaAlternativa);
        await _dbContext.SaveChangesAsync();

        return true;
    }
    
    
    /**
     * Método usado para apagar a todos as entidades do tipo RespostaAlternativaModel
     */
    public async Task<bool> ApagarTodos(int id)
    {
        RespostaAlternativaModel? respostaAlternativa = await _dbContext.RespostaAlternativaModels.FindAsync(id);

        if (respostaAlternativa == null)
            return false;
       
        _dbContext.RespostaAlternativaModels.Remove(respostaAlternativa);
        await _dbContext.SaveChangesAsync();

        return true;
    }
    
}