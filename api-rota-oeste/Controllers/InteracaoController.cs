using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api_rota_oeste.Controllers
{
    
    /// <summary>
    /// Controller responsável por gerenciar operações de Interação no sistema.
    /// </summary>
    /// <remarks>
    /// Esta controller fornece endpoints para adicionar, buscar, atualizar e remover interações.
    /// Permite operações para adicionar uma nova interação, buscar uma interação por ID, listar todas as interações, atualizar e remover interações individuais ou em massa.
    /// </remarks>
    [ApiController]
    [Route("interacao")]
    public class InteracaoController : ControllerBase
    {
        private readonly IInteracaoService _serviceInteracao;

        public InteracaoController(IInteracaoService service)
        {
            _serviceInteracao = service;
        }
        
        /// <summary>
        /// Adiciona uma nova interação ao sistema.
        /// </summary>
        /// <param name="req">Objeto que contém as informações da interação a ser adicionada.</param>
        /// <returns>Retorna os dados da interação criada.</returns>
        /// <response code="201">Interação criada com sucesso.</response>
        /// <response code="400">Erro nos dados fornecidos.</response>
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
        
        /// <summary>
        /// Busca uma interação por ID.
        /// </summary>
        /// <param name="id">ID da interação.</param>
        /// <returns>Retorna os dados da interação correspondente ao ID fornecido.</returns>
        /// <response code="200">Interação encontrada.</response>
        /// <response code="404">Interação não encontrada.</response>
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
        
        /// <summary>
        /// Lista todas as interações do sistema.
        /// </summary>
        /// <returns>Retorna uma lista de todas as interações armazenadas no banco de dados.</returns>
        /// <response code="200">Lista de interações retornada com sucesso.</response>
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
        
        /// <summary>
        /// Atualiza uma interação existente no sistema.
        /// </summary>
        /// <param name="interacao">Objeto contendo os dados atualizados da interação.</param>
        /// <returns>Retorna um status indicando o sucesso ou falha da atualização.</returns>
        /// <response code="200">Interação atualizada com sucesso.</response>
        /// <response code="204">Interação não encontrada.</response>
        /// <response code="400">Erro ao atualizar a interação.</response>
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
        
        /// <summary>
        /// Remove uma interação do sistema pelo ID.
        /// </summary>
        /// <param name="id">ID da interação a ser removida.</param>
        /// <returns>Retorna um status indicando o sucesso da remoção.</returns>
        /// <response code="204">Interação removida com sucesso.</response>
        /// <response code="404">Interação não encontrada.</response>
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
        
        /// <summary>
        /// Remove todas as interações do sistema.
        /// </summary>
        /// <returns>Retorna um status indicando o sucesso da remoção de todas as interações.</returns>
        /// <response code="204">Todos os interações removidos com sucesso.</response>
        /// <response code="404">Nenhuma interação encontrada.</response>
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
