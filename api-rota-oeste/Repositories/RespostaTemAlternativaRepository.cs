using api_rota_oeste.Data;
using api_rota_oeste.Models.RespostaTemAlternativa;
using api_rota_oeste.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Repositories;

public class RespostaTemAlternativaRepository : IRespostaTemAlternativaRepository
{

    private readonly ApiDbContext _context;

    
    public RespostaTemAlternativaRepository(ApiDbContext context)
    {
        _context = context;
    }
    
    public async Task<RespostaTemAlternativaModel?> Adicionar(RespostaTemAlternativaModel respostaTemAlternativaModel)
    {
        // Adicionando e salvando no banco de dados
        await _context.AddAsync(respostaTemAlternativaModel);
        await _context.SaveChangesAsync();
    
        // Retornando o objeto adicionado, que já estará atualizado com o ID gerado, sem a necessidade de carregar referências explicitamente
        var resultado = await _context.RespostaTemAlternativaModels
            .Include(crc => crc.Resposta)
            .Include(crc => crc.Alternativa)
            .AsSplitQuery()
            .FirstOrDefaultAsync(crc => crc.AlternativaId == respostaTemAlternativaModel.AlternativaId && crc.RespostaId == respostaTemAlternativaModel.RespostaId);

        return resultado;
    }
    
    public async Task<bool> Apagar(int respostaId, int alternativaId)
    {
        RespostaTemAlternativaModel? respostaTemAlternativa = await _context
            .RespostaTemAlternativaModels
            .FirstOrDefaultAsync(x => x.AlternativaId == alternativaId && x.RespostaId == respostaId);

        if (respostaTemAlternativa != null)
            _context.Remove(respostaTemAlternativa);
        else
            return false;

        await _context.SaveChangesAsync();

        return true;
    }

}