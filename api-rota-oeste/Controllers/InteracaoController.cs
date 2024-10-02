using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api_rota_oeste.Controllers
{
    [ApiController]
    [Route("interacao")]
    public class InteracaoController : ControllerBase
    {
        private readonly IInteracaoService _serviceInteracao;

        public InteracaoController(IInteracaoService service)
        {
            _serviceInteracao = service;
        }
        
        [HttpPost("adicionar")]
        [SwaggerOperation(
            Summary = "Adiciona uma nova interação",
            Description = "Adiciona uma interação ao sistema e retorna os dados da interação criada."
        )]
        [SwaggerResponse(201, "Interação criada com sucesso", typeof(InteracaoResponseDTO))]
        [SwaggerResponse(400, "Erro nos dados fornecidos")]
        public async Task<ActionResult<InteracaoResponseDTO>> Adicionar([FromBody] InteracaoRequestDTO req)
        {
            var intResponseDto = await _serviceInteracao.AdicionarAsync(req);

            return CreatedAtAction(
                nameof(BuscarPorId),
                new { id = intResponseDto.Id },
                intResponseDto
            );
        }
        
        [HttpGet("buscarPorId/{id}")]
        [SwaggerOperation(
            Summary = "Busca uma interação por ID",
            Description = "Retorna os dados da interação correspondente ao ID fornecido."
        )]
        [SwaggerResponse(200, "Interação encontrada", typeof(InteracaoResponseDTO))]
        [SwaggerResponse(404, "Interação não encontrada")]
        public async Task<ActionResult<InteracaoResponseDTO>> BuscarPorId(int id)
        {
            var interacao = await _serviceInteracao.BuscarPorIdAsync(id);

            return Ok(interacao);
        }
        
        [HttpGet("buscarTodos")]
        [SwaggerOperation(
            Summary = "Lista as interações",
            Description = "Lista todas as interações armazenadas no banco de dados."
        )]
        [SwaggerResponse(200, "Lista de interações retornada com sucesso", typeof(List<InteracaoModel>))]
        public async Task<ActionResult<List<InteracaoResponseDTO>>> BuscarTodos()
        {
            
            var interacoes = await _serviceInteracao.BuscarTodosAsync();
            
            return Ok(interacoes);
            
        }
        
        [HttpPatch("atualizar")]
        [SwaggerOperation(
            Summary = "Atualiza uma interação",
            Description = "Atualiza os dados da interação existente com base nas informações fornecidas."
        )]
        [SwaggerResponse(200, "Interação atualizada com sucesso")]
        [SwaggerResponse(204, "Interação não encontrada")]
        [SwaggerResponse(400, "Erro ao atualizar a interação")]
        public async Task<IActionResult> Atualizar([FromBody] InteracaoPatchDTO interacao)
        {

            var result = await _serviceInteracao.AtualizarAsync(interacao);

            if (!result)
                return BadRequest("Não foi possível atualizar a interação");

            return Ok();
        }
        
        [HttpDelete("apagarPorId/{id}")]
        [SwaggerOperation(
            Summary = "Remove uma interação",
            Description = "Deleta as informações de uma interação no banco de dados."
        )]
        [SwaggerResponse(204, "Interação removida com sucesso")]
        [SwaggerResponse(404, "Interação não encontrada")]
        public async Task<ActionResult> ApagarPorId(int id)
        {

            await _serviceInteracao.ApagarAsync(id);
            
            return NoContent();
        }        
        
        
        [HttpDelete("apagarTodos")]
        [SwaggerOperation(
            Summary = "Remove todos as interações",
            Description = "Remove todos os interações do sistema."
        )]
        [SwaggerResponse(204, "Todos os interações removidos com sucesso")]
        [SwaggerResponse(404, "Nenhum interações encontrado")]
        public async Task<IActionResult> ApagarTodos()
        {
            var status = await _serviceInteracao.ApagarTodosAsync();

            if (!status)
                return NotFound(); // Retorna 404 Not Found se nenhuma interação for encontrada

            return NoContent(); // Retorna 204 No Content se todos as interações foram removidas com sucesso
        }
        
    }
    
}
