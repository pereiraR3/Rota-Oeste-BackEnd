using api_rota_oeste.Data;
using api_rota_oeste.Models.Usuario;
using api_rota_oeste.Repositories.Interfaces;
using AutoMapper;

namespace api_rota_oeste.Repositories;

/**
 * Representa a camada de persistência de dados, isto é, em relação à classe Usuario
 */
public class UsuarioRepository : IUsuarioRepository
{

    private readonly ApiDBContext _dbContext; // Injetando contexto de DB
    private readonly IMapper _mapper; // Injetando AutoMapper

    public UsuarioRepository(ApiDBContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    /**
     * Método que serve para salvar uma nova instância da entidade usuario no banco de dados
     */
    public async Task<UsuarioResponseDTO> Adicionar(UsuarioRequestDTO request)
    {
        UsuarioModel usuario = new UsuarioModel(request);
        await _dbContext.Usuarios.AddAsync(usuario);
        await _dbContext.SaveChangesAsync();
        
        return _mapper.Map<UsuarioResponseDTO>(usuario);
    }

    /**
     * Método que serve para recuperar determinado usuario por meio de seu ID
     */
    public async Task<UsuarioModel> BuscaPorId(int id)
    {
        try
        {
            UsuarioModel usuario = await _dbContext.Usuarios.FindAsync(id);

            return usuario;

        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro ao buscar usuário: {e.Message}");
            return null;
        }
        
    }

    /**
     * Método usado para realizar deleção relacional de uma entidade do tipo Usuario 
     */
    public async Task<bool> Apagar(int id)
    {
       UsuarioModel usuario = await _dbContext.Usuarios.FindAsync(id);

       if (usuario == null)
       {
           throw new Exception($"Usuário para o ID: {id} não foi encontrado");
       }

       _dbContext.Usuarios.Remove(usuario);
       await _dbContext.SaveChangesAsync();

       return true;
    }
    
    
}