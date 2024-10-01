using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace api_rota_oeste.Controllers;

[ApiController]
[Route("cliente")]
public class ClienteController : ControllerBase
{
    
    private readonly IClienteService _clienteService;

    public ClienteController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }
    
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
