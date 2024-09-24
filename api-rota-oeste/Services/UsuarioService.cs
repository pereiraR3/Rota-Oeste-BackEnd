using api_rota_oeste.Models.Usuario;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;

namespace api_rota_oeste.Services;

/**
 * Representa a camada de serviço, isto é, a camada onde fica as regras de negócio da aplicação
 */
public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IMapper _mapper;

    public UsuarioService(IUsuarioRepository usuarioRepository, IMapper mapper)
    {
        _usuarioRepository = usuarioRepository;
        _mapper = mapper;
    }
    
    public async Task<UsuarioResponseDTO> AdicionarAsync(UsuarioRequestDTO request)
    {
        UsuarioModel usuarioModel = await _usuarioRepository.Adicionar(request);
        
        return _mapper.Map<UsuarioResponseDTO>(usuarioModel);
    }

    public async Task<UsuarioResponseDTO> BuscaPorIdAsync(int id)
    {
       UsuarioModel usuarioModel = await _usuarioRepository.BuscaPorId(id);

       if (usuarioModel == null) 
           throw new KeyNotFoundException("Usuário não encontrado");

       return _mapper.Map<UsuarioResponseDTO>(usuarioModel);
    }

    public async Task<bool> AtualizarAsync(UsuarioPatchDTO request)
    {
        var resultado = await _usuarioRepository.Atualizar(request);
        
        if(!resultado)
            throw new Exception("Erro ao atualizar usuário.");
        
        return resultado;
    }

    public async Task<bool> ApagarAsync(int id)
    {
        var resultado = await _usuarioRepository.Apagar(id);
        
        if(!resultado)
            throw new KeyNotFoundException("Usuário não encontrado.");
        
        return resultado;
    }
}