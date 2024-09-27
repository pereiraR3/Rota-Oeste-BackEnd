using api_rota_oeste.Models.Questao;
using api_rota_oeste.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api_rota_oeste.Controllers;

[ApiController]
[Route("questao")]
public class QuestaoController : ControllerBase {
    
    private readonly QuestaoService _service;

    public QuestaoController(QuestaoService service)
    {
        _service = service;
    }

    [HttpPost("criar")]
    [SwaggerOperation(Summary = "Adiciona uma nova questão", Description = "Cria uma nova questão com base nos dados fornecidos no corpo da requisição.")]
    public ActionResult criar(QuestaoRequestDTO questao){
        _service.criar(questao);
        return Created();
    }

    [HttpGet("list")]
    [SwaggerOperation(Summary = "Lista as questões", Description = "Lista todas as questões armazenadas no banco de dados.")]
    public ActionResult<List<QuestaoModel>> Listar(){
        return Ok(_service.listar());
    }

    [HttpGet("/{id:int}")]
    [SwaggerOperation(Summary = "Busca uma questão pelo ID", Description = "Obtém os detalhes de uma questão através do ID fornecido.")]
    public ActionResult<QuestaoResponseDTO> Obter(int id)
    {
        return Ok(_service.obter(id));
    }

    [HttpPut("atualizar/{id:int}")]
    [SwaggerOperation(Summary = "Atualiza as informações de uma questão", Description = "Atualiza as informações de uma questão através do ID e das novas informações.")]
    public ActionResult Atualizar(int id, QuestaoRequestDTO questao){
        _service.editar(id, questao);
        return NoContent();
    }

    [HttpDelete("excluir/{id:int}")]
    [SwaggerOperation(Summary = "Remove uma questão", Description = "Deleta as informações de uma questão no banco de dados.")]
    public ActionResult Excluir(int id) {
        _service.deletar(id);
        return NoContent();
    }
}