using api_rota_oeste.Data;
using api_rota_oeste.Models.RespostaTemAlternativa;
using api_rota_oeste.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Repositories;

/// <summary>
/// Repositório responsável pelas operações de persistência da entidade RespostaTemAlternativa no banco de dados.
/// </summary>
/// <remarks>
/// Implementa a interface <see cref="IRespostaTemAlternativaRepository"/> e define métodos para realizar operações de adição e exclusão
/// relacionadas à entidade RespostaTemAlternativa, que representa a relação entre Resposta e Alternativa.
/// </remarks>
public class RespostaTemAlternativaRepository : IRespostaTemAlternativaRepository
{

    private readonly ApiDbContext _context;

    
    public RespostaTemAlternativaRepository(ApiDbContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Adiciona uma nova instância da entidade RespostaTemAlternativa ao banco de dados.
    /// </summary>
    /// <param name="respostaTemAlternativaModel">Objeto contendo os dados da relação Resposta-Alternativa a ser adicionada.</param>
    /// <returns>Retorna a relação Resposta-Alternativa adicionada, incluindo os dados das entidades Resposta e Alternativa associadas.</returns>
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
    
    /// <summary>
    /// Remove uma instância da entidade RespostaTemAlternativa com base nos IDs da Resposta e da Alternativa.
    /// </summary>
    /// <param name="respostaId">ID da resposta associada.</param>
    /// <param name="alternativaId">ID da alternativa associada.</param>
    /// <returns>Retorna true se a relação Resposta-Alternativa for removida com sucesso, caso contrário, retorna false.</returns>
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