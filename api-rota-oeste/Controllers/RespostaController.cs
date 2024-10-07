using api_rota_oeste.Models.RespostaAlternativa;
using api_rota_oeste.Models.RespostaTemAlternativa;
using api_rota_oeste.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api_rota_oeste.Controllers;

/// <summary>
/// Controller responsável por gerenciar operações relacionadas a Respostas Alternativas no sistema.
/// </summary>
/// <remarks>
/// Esta controller fornece endpoints para adicionar, buscar, atualizar e remover respostas alternativas,
/// além de gerenciar relações entre respostas e alternativas.
/// Permite adicionar novas respostas, buscar uma resposta pelo ID, atualizar e remover respostas,
/// bem como criar e excluir relações entre Resposta e Alternativa.
/// </remarks>
[ApiController]
[Route("respostaAlternativa")]
public class RespostaController : ControllerBase
{
    private readonly IRespostaService _respostaService;

    public RespostaController(IRespostaService respostaService)
    {
        _respostaService = respostaService;
    }
    
    
    /// <summary>
    /// Adiciona uma nova resposta alternativa ao sistema.
    /// </summary>
    /// <param name="resposta">Objeto que contém as informações da resposta alternativa a ser adicionada.</param>
    /// <returns>Retorna os dados da resposta alternativa criada.</returns>
    /// <response code="201">Resposta alternativa criada com sucesso.</response>
    /// <response code="400">Dados de entrada inválidos.</response>
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
    
    /// <summary>
    /// Adiciona uma relação entre uma Resposta e uma Alternativa.
    /// </summary>
    /// <param name="respostaId">ID da resposta.</param>
    /// <param name="alternativaId">ID da alternativa.</param>
    /// <returns>Retorna os detalhes da relação criada entre Resposta e Alternativa.</returns>
    /// <response code="201">Relação Resposta-Alternativa criada com sucesso.</response>
    /// <response code="400">Dados de entrada inválidos.</response>
    /// <response code="404">Resposta ou Alternativa não encontrado.</response>
    /// <response code="500">Erro inesperado durante a criação da relação.</response>
    [HttpPost("adicionar/respostaId/{respostaId}/alternativaId/{alternativaId}")]
    [SwaggerOperation(
        Summary = "Adiciona uma relação entre Resposta e Alternativa",
        Description = "Adiciona uma relação entre um Resposta e um Alternativa e retorna os detalhes dessa relação."
    )]
    [SwaggerResponse(201, "Relação Resposta-Alternativa criada com sucesso.", typeof(RespostaTemAlternativaResponseDTO))]
    [SwaggerResponse(400, "Dados de entrada inválidos.")]
    [SwaggerResponse(404, "Resposta ou Alternativa não encontrado.")]
    public async Task<ActionResult<RespostaTemAlternativaResponseDTO>> AdicionarRespostaTemAlternativaPorId(
        
        int respostaId,
        int alternativaId
        
        )
    {
        try
        {
            // Chama o serviço para adicionar a relação entre Resposta e Cliente
            RespostaTemAlternativaResponseDTO response = await _respostaService.AdicionarRespostaTemAlternativaAsync(respostaId, alternativaId);

            // Retorna um CreatedAtAction para indicar que a entidade foi criada com sucesso
            return CreatedAtAction(nameof(Adicionar), new { respostaId = response.RespostaId, alternativaId = response.AlternativaId }, response);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            // Caso ocorra um erro inesperado, retorna InternalServerError (500)
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro inesperado. Tente novamente mais tarde.");
        }
    }
    
    /// <summary>
    /// Busca uma resposta alternativa pelo ID.
    /// </summary>
    /// <param name="id">ID da resposta alternativa.</param>
    /// <returns>Retorna os detalhes da resposta alternativa correspondente ao ID fornecido.</returns>
    /// <response code="200">Resposta alternativa encontrada.</response>
    /// <response code="404">Resposta alternativa não encontrada.</response>
    [HttpGet("buscarPorId/{id}")]
    [SwaggerResponse(200, "Resposta alternativa encontrada.", typeof(RespostaResponseDTO))]
    [SwaggerResponse(404, "Resposta alternativa não encontrada.")]
    public async Task<ActionResult<RespostaResponseDTO>> BuscarPorId(int id)
    {
        try
        {
            var respostaAlternativa = await _respostaService.BuscarPorIdAsync(id);
            return Ok(respostaAlternativa);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Atualiza as informações de uma resposta alternativa existente no sistema.
    /// </summary>
    /// <param name="resposta">Objeto contendo os dados atualizados da resposta alternativa.</param>
    /// <returns>Retorna um status indicando o sucesso ou falha da atualização.</returns>
    /// <response code="204">Resposta alternativa atualizada com sucesso.</response>
    /// <response code="404">Resposta alternativa não encontrada.</response>
    [HttpPatch("atualizar")]
    [SwaggerResponse(204, "Resposta alternativa atualizada com sucesso.")]
    [SwaggerResponse(404, "Resposta alternativa não encontrada.")]
    public async Task<ActionResult> Atualizar([FromBody] RespostaPatchDTO resposta)
    {
        try
        {
            await _respostaService.AtualizarAsync(resposta);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Remove uma relação entre uma Resposta e uma Alternativa.
    /// </summary>
    /// <param name="respostaId">ID da resposta.</param>
    /// <param name="alternativaId">ID da alternativa.</param>
    /// <returns>Retorna um status indicando o sucesso da remoção.</returns>
    /// <response code="204">Relação Resposta-Alternativa removida com sucesso.</response>
    /// <response code="404">Relação Resposta-Alternativa não encontrada.</response>
    [HttpDelete("apagar/respostaId/{respostaId}/alternativa/{alternativaId}")]
    [SwaggerOperation(
        Summary = "Remove uma relação entre Resposta e Alternativa",
        Description = "Remove o RespostaTemAlternativa associado aos 2 IDs fornecidos. Retorna 204 No Content se a remoção for bem-sucedida."
    )]
    [SwaggerResponse(204, "RespostaTemAlternativa removido com sucesso")]
    [SwaggerResponse(404, "RespostaTemAlternativa não encontrado")]
    public async Task<IActionResult> ApagarRespostaTemAlternativaPorId(int respostaId, int alternativaId)
    {
        var status = await _respostaService.ApagarRespostaTemAlternativaAsync(respostaId, alternativaId);

        if (!status)
            return NotFound(); // Retorna 404 Not Found se o respostaTemAlternativa não for encontrado

        return NoContent(); // Retorna 204 No Content se o respostaTemAlternativa foi removido com sucesso
    }
    
    /// <summary>
    /// Remove uma resposta alternativa do sistema pelo ID.
    /// </summary>
    /// <param name="id">ID da resposta alternativa a ser removida.</param>
    /// <returns>Retorna um status indicando o sucesso da remoção.</returns>
    /// <response code="204">Resposta alternativa removida com sucesso.</response>
    /// <response code="404">Resposta alternativa não encontrada.</response>
    [HttpDelete("apagarPorId/{id}")]
    [SwaggerResponse(204, "Resposta alternativa apagada com sucesso.")]
    [SwaggerResponse(404, "Resposta alternativa não encontrada.")]
    public async Task<ActionResult> ApagarPorId(int id)
    {
        try
        {
            await _respostaService.ApagarAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Remove todas as respostas alternativas do sistema.
    /// </summary>
    /// <returns>Retorna um status indicando o sucesso da remoção de todas as respostas alternativas.</returns>
    /// <response code="204">Todas as respostas alternativas foram apagadas com sucesso.</response>
    /// <response code="500">Erro ao tentar apagar todas as respostas alternativas.</response>

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
