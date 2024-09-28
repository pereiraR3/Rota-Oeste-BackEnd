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
}
