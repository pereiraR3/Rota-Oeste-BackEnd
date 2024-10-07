using api_rota_oeste.Models.Usuario;
using api_rota_oeste.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api_rota_oeste.Controllers;

/// <summary>
/// Controller responsável por gerenciar operações relacionadas aos Usuários no sistema.
/// </summary>
/// <remarks>
/// Esta controller fornece endpoints para adicionar, buscar, atualizar e remover usuários.
/// Permite criar novos usuários, buscar um usuário pelo ID, listar todos os usuários,
/// atualizar parcialmente os dados de um usuário e remover um usuário específico do sistema.
/// </remarks>
[ApiController]
[Route("usuario")]
public class UsuarioController : ControllerBase
{

    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }
    
    /// <summary>
    /// Adiciona um novo usuário ao sistema.
    /// </summary>
    /// <param name="usuario">Objeto que contém as informações do usuário a ser adicionado.</param>
    /// <returns>Retorna os dados do usuário criado.</returns>
    /// <response code="201">Usuário criado com sucesso.</response>
    /// <response code="400">Dados inválidos ou incompletos.</response>
    [HttpPost("adicionar")]
    [SwaggerOperation(
        Summary = "Adiciona um novo usuário",
        Description = "Cria um novo usuário com base nos dados fornecidos no corpo da requisição."
    )]
    [SwaggerResponse(201, "Usuário criado com sucesso", typeof(UsuarioResponseDTO))]
    [SwaggerResponse(400, "Dados inválidos ou incompletos")]
    public async Task<ActionResult<UsuarioResponseDTO>> Adicionar(UsuarioRequestDTO usuario)
    {
        UsuarioResponseDTO usuarioResponseDto = await _usuarioService.AdicionarAsync(usuario);

        // Retorna 201 Created com a URL para acessar o usuário criado
        return CreatedAtAction(
            nameof(BuscarPorId), // Nome da ação que busca o usuário pelo ID
            new { id = usuarioResponseDto.Id }, // Parâmetro para a rota
            usuarioResponseDto // O objeto criado
        );
    }
    
    /// <summary>
    /// Busca um usuário pelo ID.
    /// </summary>
    /// <param name="id">ID do usuário.</param>
    /// <returns>Retorna os detalhes do usuário correspondente ao ID fornecido.</returns>
    /// <response code="200">Usuário encontrado.</response>
    /// <response code="404">Usuário não encontrado.</response>
    [HttpGet("buscarPorId/{id:int}")]
    [SwaggerOperation(
        Summary = "Busca um usuário pelo ID",
        Description = "Obtém os detalhes do usuário através do ID fornecido."
    )]
    [SwaggerResponse(200, "Usuário encontrado", typeof(UsuarioResponseDTO))]
    [SwaggerResponse(404, "Usuário não encontrado")]
    public async Task<ActionResult<UsuarioResponseDTO>> BuscarPorId(int id)
    {
        UsuarioResponseDTO? usuario = await _usuarioService.BuscarPorIdAsync(id);

        if (usuario == null)
            return NotFound();
        
        return Ok(usuario);

    }
    
    /// <summary>
    /// Lista todos os usuários do sistema.
    /// </summary>
    /// <returns>Retorna uma lista de todos os usuários armazenados no banco de dados.</returns>
    /// <response code="200">Lista de usuários retornada com sucesso.</response>
    [HttpGet("buscarTodos")]
    [SwaggerOperation(
        Summary = "Lista os usuários",
        Description = "Lista todas os usuários armazenadas no banco de dados."
    )]
    [SwaggerResponse(200, "Lista de usuários retornada com sucesso", typeof(List<UsuarioModel>))]
    public async Task<ActionResult<List<UsuarioResponseDTO>>> BuscarTodos()
    {
        var usuarios = await _usuarioService.BuscarTodosAsync();
        
        return Ok(usuarios);
    }
    
    /// <summary>
    /// Atualiza parcialmente os dados de um usuário existente no sistema.
    /// </summary>
    /// <param name="usuario">Objeto contendo os dados atualizados do usuário.</param>
    /// <returns>Retorna um status indicando o sucesso da atualização.</returns>
    /// <response code="204">Usuário atualizado com sucesso, sem conteúdo no corpo da resposta.</response>
    /// <response code="404">Usuário não encontrado para o ID fornecido.</response>
    /// <response code="400">Requisição inválida. Verifique os dados enviados.</response>
    [HttpPatch("atualizar")]
    [SwaggerOperation(
        Summary = "Atualiza parcialmente os dados de um usuário",
        Description = "Permite a atualização parcial dos dados de um usuário com base no ID e nos campos fornecidos."
    )]
    [SwaggerResponse(204, "Usuário atualizado com sucesso, sem conteúdo no corpo da resposta.")]
    [SwaggerResponse(404, "Usuário não encontrado para o ID fornecido.")]
    [SwaggerResponse(400, "Requisição inválida. Verifique os dados enviados.")]
    public async Task<IActionResult> Atualizar(UsuarioPatchDTO usuario)
    {
        var statusResultado = await _usuarioService.AtualizarAsync(usuario);
    
        if (statusResultado == false)
            return NotFound();
    
        return NoContent();
    }
    
    /// <summary>
    /// Remove um usuário do sistema pelo ID.
    /// </summary>
    /// <param name="id">ID do usuário a ser removido.</param>
    /// <returns>Retorna um status indicando o sucesso da remoção.</returns>
    /// <response code="204">Usuário removido com sucesso.</response>
    /// <response code="404">Usuário não encontrado.</response>
    [HttpDelete("apagarId/{id}")]
    [SwaggerOperation(
        Summary = "Remove um usuário",
        Description = "Remove o usuário associado ao ID fornecido. Retorna 204 No Content se a remoção for bem-sucedida."
    )]
    [SwaggerResponse(204, "Usuário removido com sucesso")]
    [SwaggerResponse(404, "Usuário não encontrado")]
    public async Task<IActionResult> ApagarPorId(int id)
    {
        var status = await _usuarioService.ApagarAsync(id);

        if (!status)
            return NotFound(); // Retorna 404 Not Found se o usuário não for encontrado

        return NoContent(); // Retorna 204 No Content se o usuário foi removido com sucesso
    }

}