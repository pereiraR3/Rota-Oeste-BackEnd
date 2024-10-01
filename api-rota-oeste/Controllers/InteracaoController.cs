using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api_rota_oeste.Controllers;

[ApiController]
[Route("interacao")]
public class InteracaoController : ControllerBase {
        
    private readonly IInteracaoService _service;

    public InteracaoController(IInteracaoService service)
    {
        _service = service;
    }

    [HttpPost("criar")]
    [SwaggerOperation(Summary = "Adiciona uma nova interação", Description = "Cria uma nova interação com base nos dados fornecidos no corpo da requisição.")]
    public ActionResult criar(InteracaoRequestDTO interacao){
        _service.criar(interacao);
        return Ok();
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Adiciona um novo cliente",
        Description = "Adiciona um cliente ao sistema e retorna o cliente criado.")]
    [SwaggerResponse(201, "Cliente criado com sucesso")]
    public async Task<ActionResult<InteracaoResponseDTO>> Criar(InteracaoRequestDTO req)
    {
        InteracaoResponseDTO intResponseDto = await _service.CriarAsync(req);

        return CreatedAtAction(
            nameof(BuscarPorId), // Nome da ação que busca o cliente pelo ID
            new { id = intResponseDto.Id }, // Parâmetro para a rota
            intResponseDto // O objeto criado
        );
    }

    [HttpGet]
    public async Task<ActionResult<InteracaoResponseDTO>> BuscarPorId(int id)
    {
        var interacao = await _service.BuscarPorId(id);
     
        if(interacao == null) return NotFound($"A interação de id ${id} não foi encontrada");

        var intResponse = new InteracaoResponseDTO(id, interacao.ClienteId, 
            interacao.CheckListId, interacao.Data, interacao.Status);

        return Ok(intResponse);
    }

    [HttpPut]
    [SwaggerOperation(Summary = "Atualiza uma interação", Description = "Cria uma nova interação com base nos dados fornecidos no corpo da requisição.")]
    public async Task<IActionResult> Atualizar(InteracaoPatchDTO interacao)
    {
        var busca = await _service.BuscarPorId(interacao.Id);
        if(busca == null) return NoContent();

        var result = await _service.Atualizar(interacao);

        if (!result) return BadRequest("Nao foi possivel atualizar a interacao");

        return Ok();
    }
}
