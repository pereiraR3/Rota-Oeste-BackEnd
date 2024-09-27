using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api_rota_oeste.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckListController : ControllerBase
    {
        private readonly ICheckListService _checkListService;

        public CheckListController(ICheckListService checklistService)
        {
            _checkListService = checklistService;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Adiciona um novo checklist",
        Description = "Adiciona um checklist ao sistema e retorna o checklist criado.")]
        [SwaggerResponse(201, "Checklist criado com sucesso")]
        public async Task<ActionResult<CheckListResponseDTO>> Add(CheckListRequestDTO check)
        {
            CheckListResponseDTO checkListResponse = await _checkListService.AddAsync(check);

            return CreatedAtAction(
                nameof(FindById), // Nome da ação que busca o cliente pelo ID
                new { id = checkListResponse.Id }, // Parâmetro para a rota
                checkListResponse // O objeto criado
            );
        }

        [HttpPost("colecao")]
        [SwaggerOperation(Summary = "Adiciona uma coleção de checklists",
        Description = "Adiciona uma coleção de checklists ao sistema.")]
        [SwaggerResponse(200, "checklists criados com sucesso")]
        [SwaggerResponse(204, "Nenhum checklist adicionado")]
        public async Task<ActionResult<List<CheckListResponseDTO>>> AddColletction(CheckListCollectionDTO checks)
        {
            List<CheckListResponseDTO> checkResponse = await _checkListService.AddCollectionAsync(checks);

            if (checkResponse == null)
                return NoContent();

            return Ok(checkResponse);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Busca um checklist pelo ID",
        Description = "Busca o checklist associado ao ID fornecido.")]
        [SwaggerResponse(200, "Checklist encontrado com sucesso")]
        [SwaggerResponse(404, "Checklist não encontrado")]
        public async Task<ActionResult<CheckListResponseDTO>> FindById(int id)
        {
            CheckListResponseDTO? check = await _checkListService.FindByIdAsync(id);

            if (check == null)
            {
                return NotFound();
            }

            return Ok(check);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Busca todos os checklists",
        Description = "Retorna uma lista de todos os checklists do sistema.")]
        [SwaggerResponse(200, "Checklists encontrados com sucesso")]
        [SwaggerResponse(204, "Nenhum checklist encontrado")]
        public async Task<ActionResult<List<CheckListResponseDTO>>> GetAll()
        {
            List<CheckListResponseDTO> checkResponse = await _checkListService.GetAllAsync();

            if (checkResponse == null)
                return NoContent();

            return Ok(checkResponse);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove um checklist",
        Description = "Remove o checklist associado ao ID fornecido. Retorna 204 No Content se a remoção for bem-sucedida.")]
        [SwaggerResponse(204, "Checklist removido com sucesso")]
        [SwaggerResponse(404, "Checklist não encontrado")]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _checkListService.DeleteAsync(id);

            if (!status)
                return NotFound(); // Retorna 404 Not Found se o checklist não for encontrado

            return NoContent(); // Retorna 204 No Content se o checklist foi removido com sucesso
        }

        [HttpDelete]
        [SwaggerOperation(Summary = "Remove todos os checklists",
        Description = "Remove todos os checklists do sistema.")]
        [SwaggerResponse(204, "Todos os checklists removidos com sucesso")]
        [SwaggerResponse(404, "Nenhum checklist encontrado")]
        public async Task<IActionResult> DeleteAll()
        {
            var status = await _checkListService.DeleteAllAsync();

            if (!status)
                return NotFound(); // Retorna 404 Not Found se nenhum checklist for encontrado

            return NoContent(); // Retorna 204 No Content se todos os checklists foram removidos com sucesso
        }

    }
}