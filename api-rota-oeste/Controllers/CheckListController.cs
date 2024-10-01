using api_rota_oeste.Models.CheckList;
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

        public CheckListController(ICheckListService checklistService)
        {
            _checkListService = checklistService;
        }

        [HttpPost("adicionar")]
        [SwaggerOperation(Summary = "Adiciona um novo checklist",
        Description = "Adiciona um checklist ao sistema e retorna o checklist criado.")]
        [SwaggerResponse(201, "Checklist criado com sucesso")]
        public async Task<ActionResult<CheckListResponseDTO>> Adicionar(CheckListRequestDTO check)
        {
            CheckListResponseDTO checkListResponse = await _checkListService.AdicionarAsync(check);

            return CreatedAtAction(
                nameof(BuscarPorId), // Nome da ação que busca o cliente pelo ID
                new { id = checkListResponse.Id }, // Parâmetro para a rota
                checkListResponse // O objeto criado
            );
        }

        [HttpGet("buscarPorId/{id}")]
        [SwaggerOperation(Summary = "Busca um checklist pelo ID",
        Description = "Busca o checklist associado ao ID fornecido.")]
        [SwaggerResponse(200, "Checklist encontrado com sucesso")]
        [SwaggerResponse(404, "Checklist n�o encontrado")]
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
        [SwaggerOperation(Summary = "Busca todos os checklists",
        Description = "Retorna uma lista de todos os checklists do sistema.")]
        [SwaggerResponse(200, "Checklists encontrados com sucesso")]
        [SwaggerResponse(204, "Nenhum checklist encontrado")]
        public async Task<ActionResult<List<CheckListResponseDTO>>> BuscarTodos()
        {
            List<CheckListResponseDTO> checkResponse = await _checkListService.BuscarTodosAsync();

            return Ok(checkResponse);
        }

        [HttpDelete("apagarPorId/{id}")]
        [SwaggerOperation(Summary = "Remove um checklist",
        Description = "Remove o checklist associado ao ID fornecido. Retorna 204 No Content se a remo��o for bem-sucedida.")]
        [SwaggerResponse(204, "Checklist removido com sucesso")]
        [SwaggerResponse(404, "Checklist n�o encontrado")]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _checkListService.ApagarAsync(id);

            if (!status)
                return NotFound(); // Retorna 404 Not Found se o checklist n�o for encontrado

            return NoContent(); // Retorna 204 No Content se o checklist foi removido com sucesso
        }

        [HttpDelete("apagarTodos")]
        [SwaggerOperation(Summary = "Remove todos os checklists",
        Description = "Remove todos os checklists do sistema.")]
        [SwaggerResponse(204, "Todos os checklists removidos com sucesso")]
        [SwaggerResponse(404, "Nenhum checklist encontrado")]
        public async Task<IActionResult> DeleteAll()
        {
            var status = await _checkListService.ApagarTodosAsync();

            if (!status)
                return NotFound(); // Retorna 404 Not Found se nenhum checklist for encontrado

            return NoContent(); // Retorna 204 No Content se todos os checklists foram removidos com sucesso
        }

    }
}
