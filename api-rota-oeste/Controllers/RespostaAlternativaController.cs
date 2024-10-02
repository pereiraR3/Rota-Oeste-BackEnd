using api_rota_oeste.Models.RespostaAlternativa;
using api_rota_oeste.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api_rota_oeste.Controllers;

[ApiController]
[Route("respostaAlternativa")]
public class RespostaAlternativaController : ControllerBase
{
    private readonly IRespostaAlternativaService _respostaAlternativaService;

    public RespostaAlternativaController(IRespostaAlternativaService respostaAlternativaService)
    {
        _respostaAlternativaService = respostaAlternativaService;
    }
    
    [HttpPost("adicionar")]
    [SwaggerResponse(201, "Resposta alternativa criada com sucesso.", typeof(RespostaAlternativaResponseDTO))]
    [SwaggerResponse(400, "Dados de entrada inválidos.")]
    public async Task<CreatedAtActionResult> Adicionar([FromBody] RespostaAlternativaRequestDTO resposta)
    {
        RespostaAlternativaResponseDTO respostaAlternativaResponse = await _respostaAlternativaService.AdicionarAsync(resposta);

        return CreatedAtAction(
            nameof(BuscarPorId),
            new { id = respostaAlternativaResponse.Id },
            respostaAlternativaResponse
        );
    }
    
    [HttpGet("buscarPorId/{id}")]
    [SwaggerResponse(200, "Resposta alternativa encontrada.", typeof(RespostaAlternativaResponseDTO))]
    [SwaggerResponse(404, "Resposta alternativa não encontrada.")]
    public async Task<ActionResult<RespostaAlternativaResponseDTO>> BuscarPorId(int id)
    {
        try
        {
            var respostaAlternativa = await _respostaAlternativaService.BuscarPorIdAsync(id);
            return Ok(respostaAlternativa);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    [HttpPatch("atualizar")]
    [SwaggerResponse(204, "Resposta alternativa atualizada com sucesso.")]
    [SwaggerResponse(404, "Resposta alternativa não encontrada.")]
    public async Task<ActionResult> Atualizar([FromBody] RespostaAlternativaPatchDTO resposta)
    {
        var respostaAlternativa = await _respostaAlternativaService.BuscarPorIdAsync(resposta.Id);

        if (respostaAlternativa == null)
            return NotFound("RespostaAlternativa não encontrada");

        await _respostaAlternativaService.AtualizarAsync(resposta);

        return NoContent();
    }
    
    [HttpDelete("apagarPorId/{id}")]
    [SwaggerResponse(204, "Resposta alternativa apagada com sucesso.")]
    [SwaggerResponse(404, "Resposta alternativa não encontrada.")]
    public async Task<ActionResult> ApagarPorId(int id)
    {
        var respostaAlternativa = await _respostaAlternativaService.BuscarPorIdAsync(id);

        if (respostaAlternativa == null)
            return NotFound("RespostaAlternativa não encontrada");

        await _respostaAlternativaService.ApagarAsync(id);

        return NoContent();
    }
    
    [HttpDelete("apagarTodos")]
    [SwaggerResponse(204, "Todas as respostas alternativas foram apagadas com sucesso.")]
    [SwaggerResponse(500, "Erro ao tentar apagar todas as respostas alternativas.")]
    public async Task<ActionResult> ApagarTodos()
    {
        var resultado = await _respostaAlternativaService.ApagarTodosAsync();

        if (!resultado)
            throw new ApplicationException("Operação violada");

        return NoContent();
    }
}
