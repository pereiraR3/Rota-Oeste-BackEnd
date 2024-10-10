using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.ClienteRespondeCheckList;
using api_rota_oeste.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api_rota_oeste.Controllers
{
    
    
    /// <summary>
    /// Controller responsável pelas operações relacionadas a CheckList no sistema.
    /// </summary>
    /// <remarks>
    /// Esta controller fornece métodos para adicionar, buscar, atualizar e remover checklists, bem como associar um cliente a um checklist específico.
    /// Os métodos incluem suporte para a adição de novos checklists, atualização parcial de um checklist existente,
    /// exclusão de checklists e geração de relatórios.
    /// </remarks>
    /// <response code="201">Checklist criado ou relação entre Cliente e CheckList adicionada com sucesso.</response>
    /// <response code="204">Operação realizada com sucesso, sem retorno de conteúdo.</response>
    /// <response code="400">Dados de entrada inválidos.</response>
    /// <response code="404">Entidade não encontrada.</response>
    /// <response code="500">Erro interno do servidor ao processar a solicitação.</response>
    [ApiController]
    [Route("checklist")]
    [Authorize]
    public class CheckListController : ControllerBase
    {
        private readonly ICheckListService _checkListService;

        public CheckListController(
            
            ICheckListService checklistService
            
            )
        {
            _checkListService = checklistService;
        }

        /// <summary>
        /// Adiciona um novo checklist ao sistema.
        /// </summary>
        /// <param name="check">Objeto que contém as informações do checklist a ser adicionado.</param>
        /// <returns>Retorna o checklist criado.</returns>
        /// <response code="201">Checklist criado com sucesso.</response>
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
        
        /// <summary>
        /// Adiciona uma relação entre um Cliente e um CheckList.
        /// </summary>
        /// <param name="clienteId">ID do cliente.</param>
        /// <param name="checkListId">ID do checklist.</param>
        /// <returns>Retorna os detalhes da relação criada entre Cliente e CheckList.</returns>
        /// <response code="201">Relação Cliente-CheckList criada com sucesso.</response>
        /// <response code="400">Dados de entrada inválidos.</response>
        /// <response code="404">Cliente ou CheckList não encontrado.</response>
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

        /// <summary>
        /// Busca um checklist pelo ID.
        /// </summary>
        /// <param name="id">ID do checklist.</param>
        /// <returns>Retorna o checklist associado ao ID fornecido.</returns>
        /// <response code="200">Checklist encontrado com sucesso.</response>
        /// <response code="404">Checklist não encontrado.</response>
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

        /// <summary>
        /// Busca todos os checklists do sistema.
        /// </summary>
        /// <returns>Retorna uma lista de todos os checklists.</returns>
        /// <response code="200">Checklists encontrados com sucesso.</response>
        /// <response code="204">Nenhum checklist encontrado.</response>
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

        /// <summary>
        /// Atualiza as informações de um checklist.
        /// </summary>
        /// <param name="checkListPatchDto">Objeto contendo as informações a serem atualizadas no checklist.</param>
        /// <returns>Retorna um status indicando o sucesso da atualização.</returns>
        /// <response code="204">Checklist atualizado com sucesso.</response>
        /// <response code="400">Erro nos dados fornecidos.</response>
        /// <response code="404">Checklist não encontrado.</response>
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
        
        /// <summary>
        /// Remove uma relação entre um Cliente e um CheckList.
        /// </summary>
        /// <param name="clienteId">ID do cliente.</param>
        /// <param name="checkListId">ID do checklist.</param>
        /// <returns>Retorna um status indicando o sucesso da remoção.</returns>
        /// <response code="204">ClienteRespondeCheckList removido com sucesso.</response>
        /// <response code="404">ClienteRespondeCheckList não encontrado.</response>
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
        
        /// <summary>
        /// Remove um checklist pelo ID.
        /// </summary>
        /// <param name="id">ID do checklist a ser removido.</param>
        /// <returns>Retorna um status indicando o sucesso da remoção.</returns>
        /// <response code="204">Checklist removido com sucesso.</response>
        /// <response code="404">Checklist não encontrado.</response>
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
        
        /// <summary>
        /// Remove todos os checklists do sistema.
        /// </summary>
        /// <returns>Retorna um status indicando o sucesso da remoção de todos os checklists.</returns>
        /// <response code="204">Todos os checklists removidos com sucesso.</response>
        /// <response code="404">Nenhum checklist encontrado.</response>
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
        
        /// <summary>
        /// Gera um relatório geral para um checklist específico.
        /// </summary>
        /// <param name="idChecklist">ID do checklist para o qual o relatório deve ser gerado.</param>
        /// <returns>Retorna informações detalhadas sobre o checklist especificado.</returns>
        /// <response code="200">Relatório gerado com sucesso.</response>
        /// <response code="404">Checklist não encontrado ou não há dados disponíveis.</response>
        /// <response code="500">Erro ao gerar o relatório.</response>
        [HttpGet("relatorio-geral/{idChecklist}")]
        [SwaggerOperation(
            Summary = "Gera um relatório geral para um checklist específico",
            Description = "Retorna informações detalhadas sobre o checklist especificado."
        )]
        public async Task<ActionResult<List<CheckListRelatorioGeralDTO>>> GetRelatorioGeralPorCheckListId(int idChecklist)
        {
            try
            {
                var relatorio = await _checkListService.GerarRelatorioGeralAsync(idChecklist);

                if (relatorio == null || !relatorio.Any())
                {
                    return NotFound("Checklist não encontrado ou não há dados disponíveis.");
                }

                return Ok(relatorio);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro ao gerar o relatório: {ex.Message}");
            }
        }

    }
}
