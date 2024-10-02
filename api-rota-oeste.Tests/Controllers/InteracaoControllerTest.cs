using api_rota_oeste.Controllers;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace api_rota_oeste.Tests.Controllers
{
    public class InteracaoControllerTest
    {
        private readonly Mock<IInteracaoService> _interacaoServiceMock;
        private readonly InteracaoController _interacaoController;

        public InteracaoControllerTest()
        {
            _interacaoServiceMock = new Mock<IInteracaoService>();
            _interacaoController = new InteracaoController(_interacaoServiceMock.Object);
        }

        [Fact]
        public async Task Criar_DeveRetornar201Created()
        {
            // Arrange
            var interacaoRequest = new InteracaoRequestDTO(1, 1, true);
            var interacaoResponse = new InteracaoResponseDTO(1, 1, 1, true, null, null, null);

            _interacaoServiceMock.Setup(service => service.AdicionarAsync(interacaoRequest))
                .ReturnsAsync(interacaoResponse);

            // Act
            var result = await _interacaoController.Criar(interacaoRequest);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(201, createdResult.StatusCode);
            Assert.Equal(interacaoResponse, createdResult.Value);
            _interacaoServiceMock.Verify(service => service.AdicionarAsync(interacaoRequest), Times.Once);
        }

        [Fact]
        public async Task BuscarPorId_DeveRetornar200OkSeEncontrado()
        {
            // Arrange
            var interacaoId = 1;
            var interacaoResponse = new InteracaoResponseDTO(interacaoId, 1, 1, true, null, null, null);

            _interacaoServiceMock.Setup(service => service.BuscarPorIdAsync(interacaoId))
                .ReturnsAsync(interacaoResponse);

            // Act
            var result = await _interacaoController.BuscarPorId(interacaoId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(interacaoResponse, okResult.Value);
            _interacaoServiceMock.Verify(service => service.BuscarPorIdAsync(interacaoId), Times.Once);
        }

        [Fact]
        public async Task BuscarPorId_DeveRetornar404SeNaoEncontrado()
        {
            // Arrange
            var interacaoId = 1;

            _interacaoServiceMock.Setup(service => service.BuscarPorIdAsync(interacaoId))
                .ReturnsAsync((InteracaoResponseDTO)null);

            // Act
            var result = await _interacaoController.BuscarPorId(interacaoId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal("Interação não encontrada", notFoundResult.Value);
            _interacaoServiceMock.Verify(service => service.BuscarPorIdAsync(interacaoId), Times.Once);
        }

        [Fact]
        public async Task Atualizar_DeveRetornar200OkSeAtualizadoComSucesso()
        {
            // Arrange
            var interacaoPatch = new InteracaoPatchDTO(1, true);
            var interacaoResponse = new InteracaoResponseDTO(1, 1, 1, true, null, null, null);

            _interacaoServiceMock.Setup(service => service.BuscarPorIdAsync(interacaoPatch.Id))
                .ReturnsAsync(interacaoResponse);

            _interacaoServiceMock.Setup(service => service.AtualizarAsync(interacaoPatch))
                .ReturnsAsync(true);

            // Act
            var result = await _interacaoController.Atualizar(interacaoPatch);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            _interacaoServiceMock.Verify(service => service.BuscarPorIdAsync(interacaoPatch.Id), Times.Once);
            _interacaoServiceMock.Verify(service => service.AtualizarAsync(interacaoPatch), Times.Once);
        }

        [Fact]
        public async Task Atualizar_DeveRetornar204NoContentSeNaoEncontrado()
        {
            // Arrange
            var interacaoPatch = new InteracaoPatchDTO(1, true);

            _interacaoServiceMock.Setup(service => service.BuscarPorIdAsync(interacaoPatch.Id))
                .ReturnsAsync((InteracaoResponseDTO)null);

            // Act
            var result = await _interacaoController.Atualizar(interacaoPatch);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
            _interacaoServiceMock.Verify(service => service.BuscarPorIdAsync(interacaoPatch.Id), Times.Once);
            _interacaoServiceMock.Verify(service => service.AtualizarAsync(It.IsAny<InteracaoPatchDTO>()), Times.Never);
        }

        [Fact]
        public async Task Atualizar_DeveRetornar400BadRequestSeFalhaNaAtualizacao()
        {
            // Arrange
            var interacaoPatch = new InteracaoPatchDTO(1, true);
            var interacaoResponse = new InteracaoResponseDTO(1, 1, 1, true, null, null, null);

            _interacaoServiceMock.Setup(service => service.BuscarPorIdAsync(interacaoPatch.Id))
                .ReturnsAsync(interacaoResponse);

            _interacaoServiceMock.Setup(service => service.AtualizarAsync(interacaoPatch))
                .ReturnsAsync(false);

            // Act
            var result = await _interacaoController.Atualizar(interacaoPatch);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Não foi possível atualizar a interação", badRequestResult.Value);
            _interacaoServiceMock.Verify(service => service.BuscarPorIdAsync(interacaoPatch.Id), Times.Once);
            _interacaoServiceMock.Verify(service => service.AtualizarAsync(interacaoPatch), Times.Once);
        }
    }
}
