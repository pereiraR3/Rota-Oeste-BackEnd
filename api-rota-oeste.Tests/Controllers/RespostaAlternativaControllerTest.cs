using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api_rota_oeste.Controllers;
using api_rota_oeste.Models.RespostaAlternativa;
using api_rota_oeste.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace api_rota_oeste.Tests.Controllers;

public class RespostaAlternativaControllerTests
{
    private readonly Mock<IRespostaAlternativaService> _mockRespostaAlternativaService;
    private readonly RespostaAlternativaController _controller;

    public RespostaAlternativaControllerTests()
    {
        _mockRespostaAlternativaService = new Mock<IRespostaAlternativaService>();
        _controller = new RespostaAlternativaController(_mockRespostaAlternativaService.Object);
    }

    [Fact]
    public async Task Adicionar_ShouldReturnCreatedAtAction_WhenDataIsValid()
    {
        // Arrange
        var requestDto = new RespostaAlternativaRequestDTO(1, 1, 2, null);
        var responseDto = new RespostaAlternativaResponseDTO(1, 1, 1, 2, null, null, null);

        _mockRespostaAlternativaService.Setup(x => x.AdicionarAsync(requestDto)).ReturnsAsync(responseDto);

        // Act
        var result = await _controller.Adicionar(requestDto) as CreatedAtActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(201, result.StatusCode);
        Assert.Equal("BuscarPorId", result.ActionName);
        Assert.Equal(responseDto, result.Value);
    }

    [Fact]
    public async Task BuscarPorId_ShouldReturnOk_WhenRespostaAlternativaExists()
    {
        // Arrange
        var responseDto = new RespostaAlternativaResponseDTO(1, 1, 1, 2, null, null, null);

        _mockRespostaAlternativaService.Setup(x => x.BuscarPorIdAsync(1)).ReturnsAsync(responseDto);

        // Act
        var result = await _controller.BuscarPorId(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(responseDto, okResult.Value);
    }

    [Fact]
    public async Task BuscarPorId_ShouldReturnNotFound_WhenRespostaAlternativaDoesNotExist()
    {
        // Arrange
        _mockRespostaAlternativaService.Setup(x => x.BuscarPorIdAsync(1)).ThrowsAsync(new KeyNotFoundException("RespostaAlternativa não encontrada"));

        // Act
        var result = await _controller.BuscarPorId(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.Equal("RespostaAlternativa não encontrada", notFoundResult?.Value);
    }
    
    [Fact]
    public async Task Atualizar_ShouldReturnNoContent_WhenRespostaAlternativaIsUpdated()
    {
        // Arrange
        var patchDto = new RespostaAlternativaPatchDTO(1, 2, null);
        _mockRespostaAlternativaService.Setup(x => x.BuscarPorIdAsync(patchDto.Id)).ReturnsAsync(new RespostaAlternativaResponseDTO(1, 1, 1, 2, null, null, null));
        _mockRespostaAlternativaService.Setup(x => x.AtualizarAsync(patchDto)).ReturnsAsync(true);

        // Act
        var result = await _controller.Atualizar(patchDto);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Atualizar_ShouldReturnNotFound_WhenRespostaAlternativaDoesNotExist()
    {
        // Arrange
        var patchDto = new RespostaAlternativaPatchDTO(1, 2, null);
        _mockRespostaAlternativaService.Setup(x => x.BuscarPorIdAsync(patchDto.Id)).ReturnsAsync((RespostaAlternativaResponseDTO)null);

        // Act
        var result = await _controller.Atualizar(patchDto);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task ApagarPorId_ShouldReturnNoContent_WhenRespostaAlternativaIsDeleted()
    {
        // Arrange
        _mockRespostaAlternativaService.Setup(x => x.BuscarPorIdAsync(1)).ReturnsAsync(new RespostaAlternativaResponseDTO(1, 1, 1, 2, null, null, null));
        _mockRespostaAlternativaService.Setup(x => x.ApagarAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _controller.ApagarPorId(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task ApagarPorId_ShouldReturnNotFound_WhenRespostaAlternativaDoesNotExist()
    {
        // Arrange
        _mockRespostaAlternativaService.Setup(x => x.BuscarPorIdAsync(1)).ReturnsAsync((RespostaAlternativaResponseDTO)null);

        // Act
        var result = await _controller.ApagarPorId(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task ApagarTodos_ShouldReturnNoContent_WhenAllRespostaAlternativasAreDeleted()
    {
        // Arrange
        _mockRespostaAlternativaService.Setup(x => x.ApagarTodosAsync()).ReturnsAsync(true);

        // Act
        var result = await _controller.ApagarTodos();

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task ApagarTodos_ShouldThrowApplicationException_WhenDeletionFails()
    {
        // Arrange
        _mockRespostaAlternativaService.Setup(x => x.ApagarTodosAsync()).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<ApplicationException>(() => _controller.ApagarTodos());
    }
}