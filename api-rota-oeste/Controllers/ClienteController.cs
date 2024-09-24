using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace api_rota_oeste.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    /// <param name="usuario">Dados do cliente a ser adicionado.</param>
    /// <returns>Retorna o cliente criado com o status 201 Created.</returns>
    /// <response code="201">Cliente criado com sucesso.</response>
    [HttpPost]
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
    /// <param name="clientes">Dados da coleção de clientes a ser adicionada.</param>
    /// <returns>Retorna a lista de clientes criados ou 204 No Content se não houver clientes.</returns>
    /// <response code="200">Clientes criados com sucesso.</response>
    /// <response code="204">Nenhum cliente adicionado.</response>
    [HttpPost("colecao")]
    [SwaggerOperation(Summary = "Adiciona uma coleção de clientes",
        Description = "Adiciona uma coleção de clientes ao sistema.")]
    [SwaggerResponse(200, "Clientes criados com sucesso")]
    [SwaggerResponse(204, "Nenhum cliente adicionado")]
    public async Task<ActionResult<List<ClienteResponseDTO>>> AdicionarColecao(ClienteCollectionDTO clientes)
    {
        List<ClienteResponseDTO> clienteResponseDtos = await _clienteService.AdicionarColecaoAsync(clientes);
        
        if (clienteResponseDtos == null)
            return NoContent();
        
        return Ok(clienteResponseDtos);
    }

    /// <summary>
    /// Busca um cliente pelo ID.
    /// </summary>
    /// <param name="id">ID do cliente a ser buscado.</param>
    /// <returns>Retorna o cliente correspondente ou 404 Not Found se não encontrado.</returns>
    /// <response code="200">Cliente encontrado com sucesso.</response>
    /// <response code="404">Cliente não encontrado.</response>
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Busca um cliente pelo ID",
        Description = "Busca o cliente associado ao ID fornecido.")]
    [SwaggerResponse(200, "Cliente encontrado com sucesso")]
    [SwaggerResponse(404, "Cliente não encontrado")]
    public async Task<ActionResult<ClienteResponseDTO>> BuscarPorId(int id)
    {
        ClienteResponseDTO cliente = await _clienteService.BuscaPorIdAsync(id);

        if (cliente == null)
        {
            return NotFound();
        }

        return Ok(cliente);
    }
    
    /// <summary>
    /// Busca todos os clientes.
    /// </summary>
    /// <returns>Retorna a lista de todos os clientes ou 204 No Content se não houver clientes.</returns>
    /// <response code="200">Clientes encontrados com sucesso.</response>
    /// <response code="204">Nenhum cliente encontrado.</response>
    [HttpGet]
    [SwaggerOperation(Summary = "Busca todos os clientes",
        Description = "Retorna uma lista de todos os clientes do sistema.")]
    [SwaggerResponse(200, "Clientes encontrados com sucesso")]
    [SwaggerResponse(204, "Nenhum cliente encontrado")]
    public async Task<ActionResult<List<ClienteResponseDTO>>> BuscarTodos()
    {
        List<ClienteResponseDTO> clienteResponseDtos = await _clienteService.BuscaTodosAsync();

        if (clienteResponseDtos == null)
            return NoContent();
        
        return Ok(clienteResponseDtos);
    }

    /// <summary>
    /// Remove um cliente pelo ID.
    /// </summary>
    /// <param name="id">ID do cliente a ser removido.</param>
    /// <returns>Retorna 204 No Content se o cliente for removido com sucesso, ou 404 Not Found se o cliente não for encontrado.</returns>
    /// <response code="204">Cliente removido com sucesso.</response>
    /// <response code="404">Cliente não encontrado.</response>
    [HttpDelete("{id}")]
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
    /// Remove todos os clientes.
    /// </summary>
    /// <returns>Retorna 204 No Content se todos os clientes forem removidos com sucesso, ou 404 Not Found se nenhum cliente for encontrado.</returns>
    /// <response code="204">Todos os clientes removidos com sucesso.</response>
    /// <response code="404">Nenhum cliente encontrado.</response>
    [HttpDelete]
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
