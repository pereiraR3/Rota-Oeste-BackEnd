using api_rota_oeste.Data;
using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.Usuario;
using api_rota_oeste.Repositories.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Repositories;

/// <summary>
/// Repositório responsável pelas operações de persistência da entidade Usuario no banco de dados.
/// </summary>
/// <remarks>
/// Implementa a interface <see cref="IUsuarioRepository"/> e define métodos para realizar operações CRUD
/// relacionadas à entidade Usuario.
/// </remarks>
public class UsuarioRepository : IUsuarioRepository
{

    private readonly ApiDbContext _dbContext; // Injetando contexto de DB
    private readonly IMapper _mapper; // Injetando AutoMapper

    public UsuarioRepository(ApiDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Salva uma nova instância da entidade Usuario no banco de dados.
    /// </summary>
    /// <param name="usuario">Objeto contendo os dados do usuário a ser adicionado.</param>
    /// <returns>Retorna o usuário adicionado.</returns>
    public async Task<UsuarioModel?> Adicionar(UsuarioModel usuario)
    {
        await _dbContext.Usuarios.AddAsync(usuario);
        await _dbContext.SaveChangesAsync();

        return usuario;
    }

    /// <summary>
    /// Busca uma instância da entidade Usuario pelo ID.
    /// </summary>
    /// <param name="id">ID do usuário a ser buscado.</param>
    /// <returns>Retorna o usuário correspondente ao ID fornecido, ou null se não for encontrado.</returns>
    public async Task<UsuarioModel?> BuscaPorId(int id)
    {
        
        return await _dbContext.Usuarios.FindAsync(id);
        
    }

    /// <summary>
    /// Busca todas as instâncias da entidade Usuario armazenadas no banco de dados.
    /// </summary>
    /// <returns>Retorna uma lista de todos os usuários.</returns>
    public async Task<List<UsuarioModel>> BuscarTodos()
    {
        return await _dbContext.Usuarios.ToListAsync();
    }

    /// <summary>
    /// Atualiza parcialmente os dados de um usuário existente no banco de dados.
    /// </summary>
    /// <param name="request">Objeto contendo os dados a serem atualizados do usuário.</param>
    /// <returns>Retorna true se a atualização for bem-sucedida, caso contrário, retorna false.</returns>
    /// <remarks>
    /// O mapeamento de atualização deve ignorar campos nulos para garantir que apenas os campos fornecidos sejam atualizados.
    /// </remarks>
    public async Task<bool> Atualizar(UsuarioPatchDTO request)
    {
        UsuarioModel? usuarioModel = await BuscaPorId(request.Id);
        
        if(usuarioModel == null)
            return false;
        
        // O mapeamento de atualização deve ignorar campos nulos
        _mapper.Map(request, usuarioModel);
        
        _dbContext.Usuarios.Update(usuarioModel);
        await _dbContext.SaveChangesAsync();

        return true;

    }

    /// <summary>
    /// Remove uma instância da entidade Usuario pelo ID.
    /// </summary>
    /// <param name="id">ID do usuário a ser removido.</param>
    /// <returns>Retorna true se o usuário for removido com sucesso, caso contrário, retorna false.</returns>
    public async Task<bool> Apagar(int id)
    {
       UsuarioModel? usuario = await _dbContext.Usuarios.FindAsync(id);

       if (usuario == null)
           return false;
       
       _dbContext.Usuarios.Remove(usuario);
       await _dbContext.SaveChangesAsync();

       return true;
    }
    
}