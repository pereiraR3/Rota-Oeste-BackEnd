using api_rota_oeste.Controllers;
using api_rota_oeste.Models.Alternativa;
using api_rota_oeste.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class AlternativaControllerTest
{
    private readonly Mock<IAlternativaService> _alternativaServiceMock;
    private readonly AlternativaController _alternativaController;

    public AlternativaControllerTest()
    {
        _alternativaServiceMock = new Mock<IAlternativaService>();
        _alternativaController = new AlternativaController(_alternativaServiceMock.Object);
    }

    [Fact]
    public async Task Adicionar_DeveRetornarCreatedAtAction_QuandoSucesso()
    {
        // Arrange
        var alternativaRequest = new AlternativaRequestDTO(1, "Descrição de teste");
        var alternativaResponse = new AlternativaResponseDTO(1, 1, "Descrição de teste", 1, null, null);

        _alternativaServiceMock.Setup(service => service.AdicionarAsync(alternativaRequest))
            .ReturnsAsync(alternativaResponse);

        // Act
        var result = await _alternativaController.Adicionar(alternativaRequest);

        // Assert
        var actionResult = Assert.IsType<CreatedAtActionResult>(result);
        var retorno = Assert.IsType<AlternativaResponseDTO>(actionResult.Value);
        Assert.Equal(alternativaResponse.Id, retorno.Id);
    }

    [Fact]
    public async Task Adicionar_DeveRetornarNotFound_QuandoQuestaoNaoExistir()
    {
        // Arrange
        var alternativaRequest = new AlternativaRequestDTO(1, "Descrição de teste");

        _alternativaServiceMock.Setup(service => service.AdicionarAsync(alternativaRequest))
            .ThrowsAsync(new KeyNotFoundException("Questão não encontrado"));

        // Act
        var result = await _alternativaController.Adicionar(alternativaRequest);

        // Assert
        var actionResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Questão não encontrado", actionResult.Value);
    }

    [Fact]
    public async Task BuscarPorId_DeveRetornarOk_QuandoAlternativaExiste()
    {
        // Arrange
        var alternativaResponse = new AlternativaResponseDTO(1, 1, "Descrição de teste", 1, null, null);

        _alternativaServiceMock.Setup(service => service.BuscarPorIdAsync(1))
            .ReturnsAsync(alternativaResponse);

        // Act
        var result = await _alternativaController.BuscarPorId(1);

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result);
        var retorno = Assert.IsType<AlternativaResponseDTO>(actionResult.Value);
        Assert.Equal(alternativaResponse.Id, retorno.Id);
    }

    [Fact]
    public async Task BuscarPorId_DeveRetornarNotFound_QuandoAlternativaNaoExiste()
    {
        // Arrange
        _alternativaServiceMock.Setup(service => service.BuscarPorIdAsync(1))
            .ThrowsAsync(new KeyNotFoundException("Alternativa não encontrada"));

        // Act
        var result = await _alternativaController.BuscarPorId(1);

        // Assert
        var actionResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Alternativa não encontrada", actionResult.Value);
    }

    [Fact]
    public async Task BuscarTodos_DeveRetornarOk_ComListaDeAlternativas()
    {
        // Arrange
        var alternativas = new List<AlternativaResponseDTO>
        {
            new AlternativaResponseDTO(1, 1, "Alternativa 1", 1, null, null),
            new AlternativaResponseDTO(2, 1, "Alternativa 2", 2, null, null)
        };

        _alternativaServiceMock.Setup(service => service.BuscarTodosAsync())
            .ReturnsAsync(alternativas);

        // Act
        var result = await _alternativaController.BuscarTodos();

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result);
        var retorno = Assert.IsType<List<AlternativaResponseDTO>>(actionResult.Value);
        Assert.Equal(2, retorno.Count);
    }

    [Fact]
    public async Task Atualizar_DeveRetornarNoContent_QuandoSucesso()
    {
        // Arrange
        var alternativaPatch = new AlternativaPatchDTO(1, "Nova descrição");

        _alternativaServiceMock.Setup(service => service.AtualizarAsync(alternativaPatch))
            .ReturnsAsync(true);

        // Act
        var result = await _alternativaController.Atualizar(alternativaPatch);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Atualizar_DeveRetornarNotFound_QuandoAlternativaNaoExistir()
    {
        // Arrange
        var alternativaPatch = new AlternativaPatchDTO(1, "Nova descrição");

        _alternativaServiceMock.Setup(service => service.AtualizarAsync(alternativaPatch))
            .ThrowsAsync(new KeyNotFoundException("Alternativa não encontrada"));

        // Act
        var result = await _alternativaController.Atualizar(alternativaPatch);

        // Assert
        var actionResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Alternativa não encontrada", actionResult.Value);
    }

    [Fact]
    public async Task Apagar_DeveRetornarNoContent_QuandoSucesso()
    {
        // Arrange
        _alternativaServiceMock.Setup(service => service.ApagarAsync(1))
            .ReturnsAsync(true);

        // Act
        var result = await _alternativaController.Apagar(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Apagar_DeveRetornarNotFound_QuandoAlternativaNaoExistir()
    {
        // Arrange
        _alternativaServiceMock.Setup(service => service.ApagarAsync(1))
            .ThrowsAsync(new KeyNotFoundException("Alternativa não encontrada"));

        // Act
        var result = await _alternativaController.Apagar(1);

        // Assert
        var actionResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Alternativa não encontrada", actionResult.Value);
    }
}
