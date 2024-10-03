using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.ClienteRespondeCheckList;
using api_rota_oeste.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api_rota_oeste.Controllers
{
    [ApiController]
    [Route("checklist")]
    public class CheckListController : ControllerBase
    {
        private readonly ICheckListService _checkListService;

        public CheckListController(
            
            ICheckListService checklistService
            
            )
        {
            _checkListService = checklistService;
        }

        [HttpPost("adicionar")]
        [SwaggerOperation(
            Summary = "Adiciona um novo checklist",
            Description = "Adiciona um checklist ao sistema e retorna o checklist criado."
        )]
        [SwaggerResponse(201, "Checklist criado com sucesso")]
        public async Task<ActionResult<CheckListResponseDTO>> Adicionar(CheckListRequestDTO check)
        {
            CheckListResponseDTO? checkListResponse = await _checkListService.AdicionarAsync(check);

            return CreatedAtAction(
                nameof(BuscarPorId), // Nome da ação que busca o cliente pelo ID
                new { id = checkListResponse.Id }, // Parâmetro para a rota
                checkListResponse // O objeto criado
            );
        }
        
        [HttpPost("adicionar/clienteId/{clienteId}/checklistId/{checkListId}")]
        [SwaggerOperation(
            Summary = "Adiciona uma relação entre Cliente e CheckList",
            Description = "Adiciona uma relação entre um Cliente e um CheckList e retorna os detalhes dessa relação."
        )]
        [SwaggerResponse(201, "Relação Cliente-CheckList criada com sucesso.", typeof(ClienteRespondeCheckListResponseDTO))]
        [SwaggerResponse(400, "Dados de entrada inválidos.")]
        [SwaggerResponse(404, "Cliente ou CheckList não encontrado.")]
        public async Task<ActionResult<ClienteRespondeCheckListResponseDTO>> AdicionarClienteRespondePorId(int clienteId, int checkListId)
        {
            try
            {
                // Chama o serviço para adicionar a relação entre Cliente e CheckList
                ClienteRespondeCheckListResponseDTO response = await _checkListService.AdicionarClienteRespondeCheckAsync(clienteId, checkListId);

                // Retorna um CreatedAtAction para indicar que a entidade foi criada com sucesso
                return CreatedAtAction(nameof(Adicionar), new { clienteId = response.ClienteId, checkListId = response.CheckListId }, response);
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

        [HttpGet("buscarPorId/{id}")]
        [SwaggerOperation(
            Summary = "Busca um checklist pelo ID",
            Description = "Busca o checklist associado ao ID fornecido."
        )]
        [SwaggerResponse(200, "Checklist encontrado com sucesso")]
        [SwaggerResponse(404, "Checklist não encontrado")]
        public async Task<ActionResult<CheckListResponseDTO>> BuscarPorId(int id)
        {
            CheckListResponseDTO? check = await _checkListService.BuscarPorIdAsync(id);

            if (check == null)
            {
                return NotFound();
            }

            return Ok(check);
        }

        [HttpGet("buscarTodos")]
        [SwaggerOperation(
            Summary = "Busca todos os checklists",
            Description = "Retorna uma lista de todos os checklists do sistema."
        )]
        [SwaggerResponse(200, "Checklists encontrados com sucesso")]
        [SwaggerResponse(204, "Nenhum checklist encontrado")]
        public async Task<ActionResult<List<CheckListResponseDTO>>> BuscarTodos()
        {
            List<CheckListResponseDTO> checkResponse = await _checkListService.BuscarTodosAsync();

            return Ok(checkResponse);
        }

        [HttpPatch("atualizar")]
        [SwaggerOperation(
            Summary = "Atualiza as informações de um checklist",
            Description = "Atualiza as informações de um checklist através do ID e das novas informações."
        )]
        [SwaggerResponse(204, "CheckList atualizado com sucesso")]
        [SwaggerResponse(400, "Erro nos dados fornecidos")]
        [SwaggerResponse(404, "CheckList não encontrado")]
        public async Task<IActionResult> Atualizar(CheckListPatchDTO checkListPatchDto)
        {
            
            var questaoExistente = await _checkListService.BuscarPorIdAsync(checkListPatchDto.Id);
            
            if (questaoExistente == null)
                return NotFound("Questão não encontrada");

            await _checkListService.AtualizarAsync(checkListPatchDto);
            
            return NoContent();
            
        }
        
        [HttpDelete("apagar/clienteId/{clienteId}/checklistId/{checkListId}")]
        [SwaggerOperation(
            Summary = "Remove uma relação entre Cliente e CheckList",
            Description = "Remove o ClienteRespondeCheckList associado aos 2 IDs fornecidos. Retorna 204 No Content se a remoção for bem-sucedida."
        )]
        [SwaggerResponse(204, "ClienteRespondeCheckList removido com sucesso")]
        [SwaggerResponse(404, "ClienteRespondeCheckList não encontrado")]
        public async Task<IActionResult> ApagarClienteRespondePorId(int clienteId, int checkListId)
        {
            var status = await _checkListService.ApagarClienteRespondeCheckAsync(clienteId, checkListId);

            if (!status)
                return NotFound(); // Retorna 404 Not Found se o clienteRespondeCheckList não for encontrado

            return NoContent(); // Retorna 204 No Content se o clienteRespondeCheckList foi removido com sucesso
        }
        
        [HttpDelete("apagarPorId/{id}")]
        [SwaggerOperation(
            Summary = "Remove um checklist",
            Description = "Remove o checklist associado ao ID fornecido. Retorna 204 No Content se a remoção for bem-sucedida."
        )]
        [SwaggerResponse(204, "Checklist removido com sucesso")]
        [SwaggerResponse(404, "Checklist não encontrado")]
        public async Task<IActionResult> ApagarPorId(int id)
        {
            var status = await _checkListService.ApagarAsync(id);

            if (!status)
                return NotFound(); // Retorna 404 Not Found se o checklist n�o for encontrado

            return NoContent(); // Retorna 204 No Content se o checklist foi removido com sucesso
        }
        
        [HttpDelete("apagarTodos")]
        [SwaggerOperation(
            Summary = "Remove todos os checklists",
            Description = "Remove todos os checklists do sistema."
        )]
        [SwaggerResponse(204, "Todos os checklists removidos com sucesso")]
        [SwaggerResponse(404, "Nenhum checklist encontrado")]
        public async Task<IActionResult> ApagarTodos()
        {
            var status = await _checkListService.ApagarTodosAsync();

            if (!status)
                return NotFound(); // Retorna 404 Not Found se nenhum checklist for encontrado

            return NoContent(); // Retorna 204 No Content se todos os checklists foram removidos com sucesso
        }

    }
}
