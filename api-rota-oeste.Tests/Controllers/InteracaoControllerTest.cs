using api_rota_oeste.Controllers;
using api_rota_oeste.Data;
using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Models.Usuario;
using api_rota_oeste.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
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
            CheckListId: 1,
            Status: true
            //Telefone: "123456789"
        );

        // Act
        var result = _controller.criar(interacaoDto);

        // Assert
        var actionResult = Assert.IsType<OkResult>(result);
        _interacaoServiceMock.Verify(s => s.criar(interacaoDto), Times.Once);
    }

    [Fact]
    public async Task BuscarPorId()
    {

        var usuarioModel = new UsuarioModel { Id = 1, Nome = "Cliente Teste", Telefone = "123456789" };
        var clienteModel = new ClienteModel { Id = 1, Nome = "Cliente Teste", Telefone = "123456789" };
        var checkModel = new CheckListModel { Id = 1, Nome = "Cliente Teste", Usuario = usuarioModel };
        var intModel = new InteracaoModel
        {
            Id = 1,
            Status = true,
            Cliente = clienteModel,
            Data = DateTime.Now,
            CheckList = checkModel
        };

        _interacaoServiceMock.Setup(repo => repo.BuscarPorId(It.IsAny<int>()))
                .ReturnsAsync(intModel);

        var result = await _controller.BuscarPorId(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result); // Verifica se o tipo retornado é OkObjectResult
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task Atualizar()
    {
        var usuarioModel = new UsuarioModel { Id = 1, Nome = "Cliente Teste", Telefone = "123456789" };
        var clienteModel = new ClienteModel { Id = 1, Nome = "Cliente Teste", Telefone = "123456789", Usuario = usuarioModel, UsuarioId = usuarioModel.Id };
        var checkModel = new CheckListModel { Id = 1, Nome = "Cliente Teste", Usuario = usuarioModel, UsuarioId = usuarioModel.Id };
        var intModel = new InteracaoModel
        {
            Id = 1,
            Status = false,
            Cliente = clienteModel,
            ClienteId = 1,
            Data = DateTime.Now,
            CheckList = checkModel,
            CheckListId = 1
        };

        var intRequest = new InteracaoRequestDTO(1, 1, true);
        var intResponse = new InteracaoResponseDTO(1, 1, 1, DateTime.Now, true);

        _interacaoServiceMock
            .Setup(s => s.CriarAsync(intRequest))
            .ReturnsAsync(intResponse);

        await _controller.Criar(intRequest);

        var intPatch = new InteracaoPatchDTO{ Id = 1, Status = true, Data = DateTime.Now};

        _interacaoServiceMock.Setup(repo => repo.Atualizar(It.IsAny<InteracaoPatchDTO>()))
                .ReturnsAsync(true);

        // Act
        var result = await _controller.Atualizar(intPatch);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}