using Microsoft.AspNetCore.Mvc;
using api_rota_oeste.Models.Alternativa;
using api_rota_oeste.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace api_rota_oeste.Controllers;

[ApiController]
[Route("alternativa")]
public class AlternativaController : ControllerBase
{
    private readonly IAlternativaService _alternativaService;

    public AlternativaController(IAlternativaService alternativaService)
    {
        _alternativaService = alternativaService;
    }

    /// <summary>
    /// Adiciona uma nova alternativa.
    /// </summary>
    /// <param name="alternativaRequest">Objeto contendo as informações necessárias para criar uma alternativa.</param>
    /// <returns>Dados da alternativa criada.</returns>
    /// <response code="201">Alternativa criada com sucesso.</response>
    /// <response code="400">Requisição inválida.</response>
    [HttpPost("adicionar")]
    [SwaggerOperation(Summary = "Adiciona uma nova alternativa", Description = "Adiciona uma alternativa associada a uma questão.")]
    [SwaggerResponse(201, "Alternativa criada com sucesso", typeof(AlternativaResponseDTO))]
    [SwaggerResponse(400, "Requisição inválida")]
    public async Task<IActionResult> Adicionar([FromBody] AlternativaRequestDTO alternativaRequest)
    {
        try
        {
            var alternativa = await _alternativaService.AdicionarAsync(alternativaRequest);
            return CreatedAtAction(nameof(BuscarPorId), new { id = alternativa.Id }, alternativa);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Busca uma alternativa pelo ID.
    /// </summary>
    /// <param name="id">ID da alternativa a ser buscada.</param>
    /// <returns>Dados da alternativa encontrada.</returns>
    /// <response code="200">Alternativa encontrada com sucesso.</response>
    /// <response code="404">Alternativa não encontrada.</response>
    [HttpGet("buscarPorId/{id}")]
    [SwaggerOperation(Summary = "Busca uma alternativa pelo ID", Description = "Retorna uma alternativa com base no ID informado.")]
    [SwaggerResponse(200, "Alternativa encontrada com sucesso", typeof(AlternativaResponseDTO))]
    [SwaggerResponse(404, "Alternativa não encontrada")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        try
        {
            var alternativa = await _alternativaService.BuscarPorIdAsync(id);
            return Ok(alternativa);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Busca todas as alternativas.
    /// </summary>
    /// <returns>Lista de todas as alternativas.</returns>
    /// <response code="200">Lista de alternativas encontrada com sucesso.</response>
    [HttpGet("buscarTodos")]
    [SwaggerOperation(Summary = "Busca todas as alternativas", Description = "Retorna uma lista com todas as alternativas cadastradas.")]
    [SwaggerResponse(200, "Lista de alternativas encontrada com sucesso", typeof(List<AlternativaResponseDTO>))]
    public async Task<IActionResult> BuscarTodos()
    {
        var alternativas = await _alternativaService.BuscarTodosAsync();
        
        return Ok(alternativas);
    }

    /// <summary>
    /// Atualiza uma alternativa.
    /// </summary>
    /// <param name="alternativaPatch">Objeto contendo os dados para atualização da alternativa.</param>
    /// <returns>Confirmação da atualização.</returns>
    /// <response code="200">Alternativa atualizada com sucesso.</response>
    /// <response code="404">Alternativa não encontrada.</response>
    [HttpPatch("atualizar")]
    [SwaggerOperation(Summary = "Atualiza uma alternativa", Description = "Atualiza uma alternativa com base nos dados fornecidos.")]
    [SwaggerResponse(200, "Alternativa atualizada com sucesso")]
    [SwaggerResponse(404, "Alternativa não encontrada")]
    public async Task<IActionResult> Atualizar([FromBody] AlternativaPatchDTO alternativaPatch)
    {
        try
        {
            var atualizado = await _alternativaService.AtualizarAsync(alternativaPatch);
            return Ok(atualizado);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Apaga uma alternativa pelo ID.
    /// </summary>
    /// <param name="id">ID da alternativa a ser apagada.</param>
    /// <returns>Confirmação da remoção.</returns>
    /// <response code="200">Alternativa removida com sucesso.</response>
    /// <response code="404">Alternativa não encontrada.</response>
    [HttpDelete("apagarPorId/{id}")]
    [SwaggerOperation(Summary = "Apaga uma alternativa", Description = "Remove uma alternativa com base no ID informado.")]
    [SwaggerResponse(200, "Alternativa removida com sucesso")]
    [SwaggerResponse(404, "Alternativa não encontrada")]
    public async Task<IActionResult> Apagar(int id)
    {
        try
        {
            var apagado = await _alternativaService.ApagarAsync(id);
            return Ok(apagado);
        }
        
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    
}
