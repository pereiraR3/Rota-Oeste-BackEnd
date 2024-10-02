using api_rota_oeste.Models.Questao;
using api_rota_oeste.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api_rota_oeste.Controllers
{
    [ApiController]
    [Route("questao")]
    public class QuestaoController : ControllerBase
    {
        private readonly IQuestaoService _service;

        public QuestaoController(IQuestaoService service)
        {
            _service = service;
        }
        
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
            
            if (questao == null)
                return NotFound("Questão não encontrada");

            return Ok(questao);
        }
        
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
            var questaoExistente = await _service.BuscarPorIdAsync(questao.Id);
            
            if (questaoExistente == null)
                return NotFound("Questão não encontrada");

            await _service.AtualizarAsync(questao);
            
            return NoContent();
        }
        
        [HttpDelete("apagarPorId/{id}")]
        [SwaggerOperation(
            Summary = "Remove uma questão",
            Description = "Deleta as informações de uma questão no banco de dados."
        )]
        [SwaggerResponse(204, "Questão removida com sucesso")]
        [SwaggerResponse(404, "Questão não encontrada")]
        public async Task<ActionResult> Excluir(int id)
        {
            var questaoExistente = await _service.BuscarPorIdAsync(id);
            if (questaoExistente == null)
                return NotFound("Questão não encontrada");

            await _service.ApagarAsync(id);
            return NoContent();
        }
    }
}
