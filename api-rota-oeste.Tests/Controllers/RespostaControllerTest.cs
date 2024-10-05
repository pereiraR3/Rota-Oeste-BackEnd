using System;
using api_rota_oeste.Controllers;
using api_rota_oeste.Models.RespostaAlternativa;
using api_rota_oeste.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using api_rota_oeste.Models.RespostaTemAlternativa;

namespace api_rota_oeste.Tests.Controllers;

public class RespostaControllerTests
{
    private readonly Mock<IRespostaService> _mockRespostaAlternativaService;
    private readonly RespostaController _controller;

    public RespostaControllerTests()
    {
        _mockRespostaAlternativaService = new Mock<IRespostaService>();
        _controller = new RespostaController(_mockRespostaAlternativaService.Object);
    }

    [Fact]
    public async Task Adicionar_ShouldReturnCreatedAtAction_WhenDataIsValid()
    {
        // Arrange
        var requestDto = new RespostaRequestDTO(1, 1, null);
        var responseDto = new RespostaResponseDTO(1, 1, 1, null, null, null, new List<RespostaTemAlternativaResponseDTO>());

        _mockRespostaAlternativaService.Setup(x => x.AdicionarAsync(requestDto)).ReturnsAsync(responseDto);

        // Act
        var result = await _controller.Adicionar(requestDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(201, createdResult.StatusCode);
        Assert.Equal("BuscarPorId", createdResult.ActionName);
        Assert.Equal(responseDto, createdResult.Value);
    }

    [Fact]
    public async Task BuscarPorId_ShouldReturnOk_WhenRespostaAlternativaExists()
    {
        // Arrange
        var responseDto = new RespostaResponseDTO(1, 1, 1, null, null, null, new List<RespostaTemAlternativaResponseDTO>());

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
        _mockRespostaAlternativaService.Setup(x => x.BuscarPorIdAsync(1)).ThrowsAsync(new KeyNotFoundException("Resposta alternativa não encontrada"));

        // Act
        var result = await _controller.BuscarPorId(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal("Resposta alternativa não encontrada", notFoundResult.Value);
    }

    [Fact]
    public async Task Atualizar_ShouldReturnNoContent_WhenRespostaAlternativaIsUpdated()
    {
        // Arrange
        var patchDto = new RespostaPatchDTO(1, null);
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
        var patchDto = new RespostaPatchDTO(5, null);
        _mockRespostaAlternativaService.Setup(x => x.AtualizarAsync(patchDto)).ThrowsAsync(new KeyNotFoundException("RespostaAlternativa não encontrada"));

        // Act
        var result = await _controller.Atualizar(patchDto);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("RespostaAlternativa não encontrada", notFoundResult.Value);
    }

    [Fact]
    public async Task ApagarPorId_ShouldReturnNoContent_WhenRespostaAlternativaIsDeleted()
    {
        // Arrange
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
        _mockRespostaAlternativaService.Setup(x => x.ApagarAsync(1)).ThrowsAsync(new KeyNotFoundException("Resposta alternativa não encontrada"));

        // Act
        var result = await _controller.ApagarPorId(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Resposta alternativa não encontrada", notFoundResult.Value);
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
