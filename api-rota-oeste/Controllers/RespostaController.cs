using api_rota_oeste.Models.RespostaAlternativa;
using api_rota_oeste.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api_rota_oeste.Controllers;

[ApiController]
[Route("respostaAlternativa")]
public class RespostaController : ControllerBase
{
    private readonly IRespostaService _respostaService;

    public RespostaController(IRespostaService respostaService)
    {
        _respostaService = respostaService;
    }
    
    [HttpPost("adicionar")]
    [SwaggerResponse(201, "Resposta alternativa criada com sucesso.", typeof(RespostaResponseDTO))]
    [SwaggerResponse(400, "Dados de entrada inválidos.")]
    public async Task<CreatedAtActionResult> Adicionar([FromBody] RespostaRequestDTO resposta)
    {
        RespostaResponseDTO respostaResponse = await _respostaService.AdicionarAsync(resposta);

        return CreatedAtAction(
            nameof(BuscarPorId),
            new { id = respostaResponse.Id },
            respostaResponse
        );
    }
    
    [HttpGet("buscarPorId/{id}")]
    [SwaggerResponse(200, "Resposta alternativa encontrada.", typeof(RespostaResponseDTO))]
    [SwaggerResponse(404, "Resposta alternativa não encontrada.")]
    public async Task<ActionResult<RespostaResponseDTO>> BuscarPorId(int id)
    {
     
        var respostaAlternativa = await _respostaService.BuscarPorIdAsync(id);
        
        return Ok(respostaAlternativa);
   
    }
    
    [HttpPatch("atualizar")]
    [SwaggerResponse(204, "Resposta alternativa atualizada com sucesso.")]
    [SwaggerResponse(404, "Resposta alternativa não encontrada.")]
    public async Task<ActionResult> Atualizar([FromBody] RespostaPatchDTO resposta)
    {
        
        await _respostaService.AtualizarAsync(resposta);

        return NoContent();
    }
    
    [HttpDelete("apagarPorId/{id}")]
    [SwaggerResponse(204, "Resposta alternativa apagada com sucesso.")]
    [SwaggerResponse(404, "Resposta alternativa não encontrada.")]
    public async Task<ActionResult> ApagarPorId(int id)
    {

        await _respostaService.ApagarAsync(id);

        return NoContent();
    }
    
    [HttpDelete("apagarTodos")]
    [SwaggerResponse(204, "Todas as respostas alternativas foram apagadas com sucesso.")]
    [SwaggerResponse(500, "Erro ao tentar apagar todas as respostas alternativas.")]
    public async Task<ActionResult> ApagarTodos()
    {
        var resultado = await _respostaService.ApagarTodosAsync();

        if (!resultado)
            throw new ApplicationException("Operação violada");

        return NoContent();
    }
}
