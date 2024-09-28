using api_rota_oeste.Controllers;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace api_rota_oeste.Tests.Controllers;

public class InteracaoControllerTest
{
    private readonly Mock<IInteracaoService> _interacaoServiceMock;
    private readonly InteracaoController _controller;

    public InteracaoControllerTest()
    {
        _interacaoServiceMock = new Mock<IInteracaoService>();
        _controller = new InteracaoController(_interacaoServiceMock.Object);
    }

    [Fact]
    public void Criar_DeveRetornarOkResult()
    {
        // Arrange
        var interacaoDto = new InteracaoRequestDTO(
            ClienteId: 1,
            Status: true,
            Telefone: "123456789"
        );

        // Act
        var result = _controller.criar(interacaoDto);

        // Assert
        var actionResult = Assert.IsType<OkResult>(result);
        _interacaoServiceMock.Verify(s => s.criar(interacaoDto), Times.Once);
    }
}