using api_rota_oeste.Models.Questao;
using api_rota_oeste.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api_rota_oeste.Controllers
{
    
    /// <summary>
    /// Controller responsável por gerenciar operações relacionadas a Questões no sistema.
    /// </summary>
    /// <remarks>
    /// Esta controller fornece endpoints para adicionar, buscar, atualizar e remover questões.
    /// Permite operações para criar novas questões, buscar uma questão pelo ID, listar todas as questões,
    /// atualizar uma questão existente e removê-la do sistema.
    /// </remarks>
    [ApiController]
    [Route("questao")]
    [Authorize]
    public class QuestaoController : ControllerBase
    {
        private readonly IQuestaoService _service;

        public QuestaoController(IQuestaoService service)
        {
            _service = service;
        }
        
        /// <summary>
        /// Adiciona uma nova questão ao sistema.
        /// </summary>
        /// <param name="questao">Objeto que contém as informações da questão a ser adicionada.</param>
        /// <returns>Retorna os dados da questão criada.</returns>
        /// <response code="200">Questão adicionada com sucesso.</response>
        /// <response code="400">Erro nos dados fornecidos.</response>
        [HttpPost("adicionar")]
        [SwaggerOperation(
            Summary = "Adiciona uma nova questão",
            Description = "Cria uma nova questão com base nos dados fornecidos no corpo da requisição."
        )]
        [SwaggerResponse(200, "Questão adicionada com sucesso")]
        [SwaggerResponse(400, "Erro nos dados fornecidos")]
        public async Task<CreatedAtActionResult> Adicionar([FromBody] QuestaoRequestDTO questao)
        {
            
            QuestaoResponseDTO questaoResponseDto = await _service.AdicionarAsync(questao);

            // Retorna 201 Created com a URL para acessar o usuário criado
            return CreatedAtAction(
                nameof(BuscarPorId), // Nome da ação que busca o usuário pelo ID
                new { id = questaoResponseDto.Id }, // Parâmetro para a rota
                questaoResponseDto // O objeto criado
            );
            
        }
        
        /// <summary>
        /// Busca uma questão pelo ID.
        /// </summary>
        /// <param name="id">ID da questão.</param>
        /// <returns>Retorna os detalhes da questão correspondente ao ID fornecido.</returns>
        /// <response code="200">Questão encontrada.</response>
        /// <response code="404">Questão não encontrada.</response>
        [HttpGet("buscarPorId/{id}")]
        [SwaggerOperation(
            Summary = "Busca uma questão pelo ID",
            Description = "Obtém os detalhes de uma questão através do ID fornecido."
        )]
        [SwaggerResponse(200, "Questão encontrada", typeof(QuestaoResponseDTO))]
        [SwaggerResponse(404, "Questão não encontrada")]
        public async Task<ActionResult<QuestaoResponseDTO>> BuscarPorId(int id)
        {
            var questao = await _service.BuscarPorIdAsync(id);

            return Ok(questao);
        }
        
        /// <summary>
        /// Lista todas as questões do sistema.
        /// </summary>
        /// <returns>Retorna uma lista de todas as questões armazenadas no banco de dados.</returns>
        /// <response code="200">Lista de questões retornada com sucesso.</response>
        [HttpGet("buscarTodos")]
        [SwaggerOperation(
            Summary = "Lista as questões",
            Description = "Lista todas as questões armazenadas no banco de dados."
        )]
        [SwaggerResponse(200, "Lista de questões retornada com sucesso", typeof(List<QuestaoModel>))]
        public async Task<ActionResult<List<QuestaoResponseDTO>>> BuscarTodos()
        {
            
            var questoes = await _service.BuscarTodosAsync();
            
            return Ok(questoes);
            
        }
        
        /// <summary>
        /// Atualiza as informações de uma questão existente no sistema.
        /// </summary>
        /// <param name="questao">Objeto contendo os dados atualizados da questão.</param>
        /// <returns>Retorna um status indicando o sucesso da atualização.</returns>
        /// <response code="204">Questão atualizada com sucesso.</response>
        /// <response code="400">Erro nos dados fornecidos.</response>
        /// <response code="404">Questão não encontrada.</response>
        [HttpPatch("atualizar")]
        [SwaggerOperation(
            Summary = "Atualiza as informações de uma questão",
            Description = "Atualiza as informações de uma questão através do ID e das novas informações."
        )]
        [SwaggerResponse(204, "Questão atualizada com sucesso")]
        [SwaggerResponse(400, "Erro nos dados fornecidos")]
        [SwaggerResponse(404, "Questão não encontrada")]
        public async Task<ActionResult> Atualizar([FromBody] QuestaoPatchDTO questao)
        {

            await _service.AtualizarAsync(questao);
            
            return NoContent();
        }
        
        /// <summary>
        /// Remove uma questão do sistema pelo ID.
        /// </summary>
        /// <param name="id">ID da questão a ser removida.</param>
        /// <returns>Retorna um status indicando o sucesso da remoção.</returns>
        /// <response code="204">Questão removida com sucesso.</response>
        /// <response code="404">Questão não encontrada.</response>
        [HttpDelete("apagarPorId/{id}")]
        [SwaggerOperation(
            Summary = "Remove uma questão",
            Description = "Deleta as informações de uma questão no banco de dados."
        )]
        [SwaggerResponse(204, "Questão removida com sucesso")]
        [SwaggerResponse(404, "Questão não encontrada")]
        public async Task<ActionResult> ApagarPorId(int id)
        {
            
            await _service.ApagarAsync(id);
            
            return NoContent();
            
        }
    }
}
