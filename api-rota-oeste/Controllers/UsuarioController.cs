using api_rota_oeste.Models.Usuario;
using api_rota_oeste.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api_rota_oeste.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{

    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioController(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    /// <summary>
    /// Adiciona um novo usuário ao sistema.
    /// </summary>
    /// <param name="usuario">Objeto de requisição que contém os dados do usuário a ser criado.</param>
    /// <returns>Retorna os detalhes do usuário criado.</returns>
    /// <response code="201">Usuário criado com sucesso.</response>
    /// <response code="400">Requisição inválida ou dados incompletos.</response>
    [HttpPost]
    [SwaggerOperation(Summary = "Adiciona um novo usuário", Description = "Cria um novo usuário com base nos dados fornecidos no corpo da requisição.")]
    [SwaggerResponse(201, "Usuário criado com sucesso", typeof(UsuarioResponseDTO))]
    [SwaggerResponse(400, "Dados inválidos ou incompletos")]
    public async Task<ActionResult<UsuarioResponseDTO>> Adicionar(UsuarioRequestDTO usuario)
    {
        UsuarioResponseDTO usuarioResponseDto = await _usuarioRepository.Adicionar(usuario);

        // Retorna 201 Created com a URL para acessar o usuário criado
        return CreatedAtAction(
            nameof(_usuarioRepository.BuscaPorId), // Nome da ação que busca o usuário pelo ID
            new { id = usuarioResponseDto.Id }, // Parâmetro para a rota
            usuarioResponseDto // O objeto criado
        );
    }

    /// <summary>
    /// Obtém um usuário pelo ID.
    /// </summary>
    /// <param name="id">ID do usuário.</param>
    /// <returns>Retorna os detalhes do usuário solicitado.</returns>
    /// <response code="200">Usuário encontrado com sucesso.</response>
    /// <response code="404">Usuário não encontrado.</response>
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Busca um usuário pelo ID", Description = "Obtém os detalhes do usuário através do ID fornecido.")]
    [SwaggerResponse(200, "Usuário encontrado", typeof(UsuarioResponseDTO))]
    [SwaggerResponse(404, "Usuário não encontrado")]
    public async Task<ActionResult<UsuarioResponseDTO>> ObterPorId(int id)
    {
        UsuarioModel usuario = await _usuarioRepository.BuscaPorId(id);

        if (usuario == null)
        {
            return NotFound();
        }

        return Ok(usuario);

    }
    
    /// <summary>
    /// Atualiza parcialmente os dados de um usuário.
    /// </summary>
    /// <param name="usuario">Objeto contendo os dados a serem atualizados no usuário.</param>
    /// <returns>Retorna um status indicando o sucesso ou falha da operação.</returns>
    /// <response code="204">Usuário atualizado com sucesso, sem conteúdo no corpo da resposta.</response>
    /// <response code="404">Usuário não encontrado para o ID fornecido.</response>
    /// <response code="400">Requisição inválida. Verifique os dados enviados.</response>
    [HttpPatch]
    [SwaggerOperation(Summary = "Atualiza parcialmente os dados de um usuário", Description = "Permite a atualização parcial dos dados de um usuário com base no ID e nos campos fornecidos.")]
    [SwaggerResponse(204, "Usuário atualizado com sucesso, sem conteúdo no corpo da resposta.")]
    [SwaggerResponse(404, "Usuário não encontrado para o ID fornecido.")]
    [SwaggerResponse(400, "Requisição inválida. Verifique os dados enviados.")]
    public async Task<IActionResult> Atualizar(UsuarioPatchDTO usuario)
    {
        var statusResultado = await _usuarioRepository.Atualizar(usuario);
    
        if (statusResultado == false)
            return NotFound();
    
        return NoContent();
    }

    /// <summary>
    /// Remove um usuário do sistema.
    /// </summary>
    /// <param name="id">ID do usuário a ser removido.</param>
    /// <returns>Retorna 204 No Content se o usuário for removido com sucesso, ou 404 Not Found se o usuário não for encontrado.</returns>
    /// <response code="204">Usuário removido com sucesso.</response>
    /// <response code="404">Usuário não encontrado.</response>
    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Remove um usuário",
        Description =
            "Remove o usuário associado ao ID fornecido. Retorna 204 No Content se a remoção for bem-sucedida.")]
    [SwaggerResponse(204, "Usuário removido com sucesso")]
    [SwaggerResponse(404, "Usuário não encontrado")]
    public async Task<IActionResult> Apagar(int id)
    {
        var status = await _usuarioRepository.Apagar(id);

        if (!status)
            return NotFound(); // Retorna 404 Not Found se o usuário não for encontrado

        return NoContent(); // Retorna 204 No Content se o usuário foi removido com sucesso
    }

}