using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace api_rota_oeste.Controllers;

/// <summary>
/// Controller responsável por gerenciar operações de Cliente no sistema.
/// </summary>
/// <remarks>
/// Esta controller fornece endpoints para adicionar, buscar e remover clientes.
/// Permite adicionar um cliente individual ou uma coleção de clientes, buscar clientes pelo ID ou buscar todos,
/// além de operações para remoção individual ou em massa.
/// </remarks>
[ApiController]
[Route("cliente")]
public class ClienteController : ControllerBase
{
    
    private readonly IClienteService _clienteService;

    public ClienteController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }
    
    /// <summary>
    /// Adiciona um novo cliente ao sistema.
    /// </summary>
    /// <param name="usuario">Objeto que contém as informações do cliente a ser adicionado.</param>
    /// <returns>Retorna o cliente criado.</returns>
    /// <response code="201">Cliente criado com sucesso.</response>
    [HttpPost("adicionar")]
    [SwaggerOperation(Summary = "Adiciona um novo cliente",
        Description = "Adiciona um cliente ao sistema e retorna o cliente criado.")]
    [SwaggerResponse(201, "Cliente criado com sucesso")]
    public async Task<ActionResult<ClienteResponseDTO>> Adicionar(ClienteRequestDTO usuario)
    {
        ClienteResponseDTO clienteResponseDto = await _clienteService.AdicionarAsync(usuario);

        return CreatedAtAction(
            nameof(BuscarPorId), // Nome da ação que busca o cliente pelo ID
            new { id = clienteResponseDto.Id }, // Parâmetro para a rota
            clienteResponseDto // O objeto criado
        );
    }
    
    /// <summary>
    /// Adiciona uma coleção de clientes ao sistema.
    /// </summary>
    /// <param name="clientes">Objeto que contém a coleção de clientes a serem adicionados.</param>
    /// <returns>Retorna a lista de clientes criados.</returns>
    /// <response code="200">Clientes criados com sucesso.</response>
    /// <response code="204">Nenhum cliente adicionado.</response>
    [HttpPost("adicionarColecao")]
    [SwaggerOperation(Summary = "Adiciona uma coleção de clientes",
        Description = "Adiciona uma coleção de clientes ao sistema.")]
    [SwaggerResponse(200, "Clientes criados com sucesso")]
    [SwaggerResponse(204, "Nenhum cliente adicionado")]
    public async Task<ActionResult<List<ClienteResponseDTO>>> AdicionarColecao(ClienteCollectionDTO clientes)
    {
        List<ClienteResponseDTO> clienteResponseDtos = await _clienteService.AdicionarColecaoAsync(clientes);
        
        return Ok(clienteResponseDtos);
    }
    
    /// <summary>
    /// Busca um cliente pelo ID.
    /// </summary>
    /// <param name="id">ID do cliente.</param>
    /// <returns>Retorna o cliente associado ao ID fornecido.</returns>
    /// <response code="200">Cliente encontrado com sucesso.</response>
    /// <response code="404">Cliente não encontrado.</response>
    [HttpGet("buscrPorId/{id}")]
    [SwaggerOperation(Summary = "Busca um cliente pelo ID",
        Description = "Busca o cliente associado ao ID fornecido.")]
    [SwaggerResponse(200, "Cliente encontrado com sucesso")]
    [SwaggerResponse(404, "Cliente não encontrado")]
    public async Task<ActionResult<ClienteResponseDTO>> BuscarPorId(int id)
    {
        ClienteResponseDTO? cliente = await _clienteService.BuscarPorIdAsync(id);

        if (cliente == null)
            return NotFound();
        
        return Ok(cliente);
    }
    
    /// <summary>
    /// Busca todos os clientes do sistema.
    /// </summary>
    /// <returns>Retorna uma lista de todos os clientes.</returns>
    /// <response code="200">Clientes encontrados com sucesso.</response>
    /// <response code="204">Nenhum cliente encontrado.</response>
    [HttpGet("buscarTodos")]
    [SwaggerOperation(Summary = "Busca todos os clientes",
        Description = "Retorna uma lista de todos os clientes do sistema.")]
    [SwaggerResponse(200, "Clientes encontrados com sucesso")]
    [SwaggerResponse(204, "Nenhum cliente encontrado")]
    public async Task<ActionResult<List<ClienteResponseDTO>>> BuscarTodos()
    {
        List<ClienteResponseDTO> clienteResponseDtos = await _clienteService.BuscarTodosAsync();
        
        return Ok(clienteResponseDtos);
    }
    
    /// <summary>
    /// Remove um cliente pelo ID.
    /// </summary>
    /// <param name="id">ID do cliente a ser removido.</param>
    /// <returns>Retorna um status indicando o sucesso da remoção.</returns>
    /// <response code="204">Cliente removido com sucesso.</response>
    /// <response code="404">Cliente não encontrado.</response>
    [HttpDelete("apagarPorId/{id}")]
    [SwaggerOperation(Summary = "Remove um cliente",
        Description = "Remove o cliente associado ao ID fornecido. Retorna 204 No Content se a remoção for bem-sucedida.")]
    [SwaggerResponse(204, "Cliente removido com sucesso")]
    [SwaggerResponse(404, "Cliente não encontrado")]
    public async Task<IActionResult> Apagar(int id)
    {
        var status = await _clienteService.ApagarAsync(id);

        if (!status)
            return NotFound(); // Retorna 404 Not Found se o cliente não for encontrado

        return NoContent(); // Retorna 204 No Content se o cliente foi removido com sucesso
    }
    
    /// <summary>
    /// Remove todos os clientes do sistema.
    /// </summary>
    /// <returns>Retorna um status indicando o sucesso da remoção de todos os clientes.</returns>
    /// <response code="204">Todos os clientes removidos com sucesso.</response>
    /// <response code="404">Nenhum cliente encontrado.</response>
    [HttpDelete("apagarTodos")]
    [SwaggerOperation(Summary = "Remove todos os clientes",
        Description = "Remove todos os clientes do sistema.")]
    [SwaggerResponse(204, "Todos os clientes removidos com sucesso")]
    [SwaggerResponse(404, "Nenhum cliente encontrado")]
    public async Task<IActionResult> ApagarTodos()
    {
        var status = await _clienteService.ApagarTodosAsync();

        if (!status)
            return NotFound(); // Retorna 404 Not Found se nenhum cliente for encontrado

        return NoContent(); // Retorna 204 No Content se todos os clientes foram removidos com sucesso
    }
}
