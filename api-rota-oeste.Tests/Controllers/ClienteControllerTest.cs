using api_rota_oeste.Controllers;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace api_rota_oeste.Tests.Controllers
{
    public class ClienteControllerTest
    {
        private readonly Mock<IClienteService> _clienteServiceMock;
        private readonly ClienteController _clienteController;

        public ClienteControllerTest()
        {
            _clienteServiceMock = new Mock<IClienteService>();
            _clienteController = new ClienteController(_clienteServiceMock.Object);
        }

        [Fact]
        public async Task Adicionar_DeveRetornar201Created()
        {
            // Arrange
            var clienteRequest = new ClienteRequestDTO(1, "Teste", "123456789", null);
            var clienteResponse = new ClienteResponseDTO(1, 1, "Teste", "123456789", null, null);

            _clienteServiceMock.Setup(service => service.AdicionarAsync(clienteRequest))
                .ReturnsAsync(clienteResponse);

            // Act
            var result = await _clienteController.Adicionar(clienteRequest);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(201, createdResult.StatusCode);
            Assert.Equal(clienteResponse, createdResult.Value);
            _clienteServiceMock.Verify(service => service.AdicionarAsync(clienteRequest), Times.Once);
        }

        [Fact]
        public async Task AdicionarColecao_DeveRetornar200Ok()
        {
            // Arrange
            var clienteCollection = new ClienteCollectionDTO(new List<ClienteRequestDTO>
            {
                new ClienteRequestDTO(1, "Cliente 1", "123456789", null),
                new ClienteRequestDTO(2, "Cliente 2", "987654321", null)
            });

            var clienteResponses = new List<ClienteResponseDTO>
            {
                new ClienteResponseDTO(1, 1, "Cliente 1", "123456789", null, null),
                new ClienteResponseDTO(2, 2, "Cliente 2", "987654321", null, null)
            };

            _clienteServiceMock.Setup(service => service.AdicionarColecaoAsync(clienteCollection))
                .ReturnsAsync(clienteResponses);

            // Act
            var result = await _clienteController.AdicionarColecao(clienteCollection);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(clienteResponses, okResult.Value);
            _clienteServiceMock.Verify(service => service.AdicionarColecaoAsync(clienteCollection), Times.Once);
        }

        [Fact]
        public async Task BuscarPorId_DeveRetornar200Ok_SeEncontrado()
        {
            // Arrange
            var clienteId = 1;
            var clienteResponse = new ClienteResponseDTO(clienteId, 1, "Cliente Teste", "123456789", null, null);

            _clienteServiceMock.Setup(service => service.BuscarPorIdAsync(clienteId))
                .ReturnsAsync(clienteResponse);

            // Act
            var result = await _clienteController.BuscarPorId(clienteId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(clienteResponse, okResult.Value);
            _clienteServiceMock.Verify(service => service.BuscarPorIdAsync(clienteId), Times.Once);
        }

        [Fact]
        public async Task BuscarPorId_DeveRetornar404NotFound_SeNaoEncontrado()
        {
            // Arrange
            var clienteId = 1;

            _clienteServiceMock.Setup(service => service.BuscarPorIdAsync(clienteId))
                .ReturnsAsync((ClienteResponseDTO)null);

            // Act
            var result = await _clienteController.BuscarPorId(clienteId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
            _clienteServiceMock.Verify(service => service.BuscarPorIdAsync(clienteId), Times.Once);
        }

        [Fact]
        public async Task BuscarTodos_DeveRetornar200Ok_SeClientesEncontrados()
        {
            // Arrange
            var clientesResponse = new List<ClienteResponseDTO>
            {
                new ClienteResponseDTO(1, 1, "Cliente 1", "123456789", null, null),
                new ClienteResponseDTO(2, 2, "Cliente 2", "987654321", null, null)
            };

            _clienteServiceMock.Setup(service => service.BuscarTodosAsync())
                .ReturnsAsync(clientesResponse);

            // Act
            var result = await _clienteController.BuscarTodos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(clientesResponse, okResult.Value);
            _clienteServiceMock.Verify(service => service.BuscarTodosAsync(), Times.Once);
        }

        [Fact]
        public async Task Apagar_DeveRetornar204NoContent_SeRemovidoComSucesso()
        {
            // Arrange
            var clienteId = 1;

            _clienteServiceMock.Setup(service => service.ApagarAsync(clienteId))
                .ReturnsAsync(true);

            // Act
            var result = await _clienteController.Apagar(clienteId);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
            _clienteServiceMock.Verify(service => service.ApagarAsync(clienteId), Times.Once);
        }

        [Fact]
        public async Task Apagar_DeveRetornar404NotFound_SeClienteNaoEncontrado()
        {
            // Arrange
            var clienteId = 1;

            _clienteServiceMock.Setup(service => service.ApagarAsync(clienteId))
                .ReturnsAsync(false);

            // Act
            var result = await _clienteController.Apagar(clienteId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            _clienteServiceMock.Verify(service => service.ApagarAsync(clienteId), Times.Once);
        }

        [Fact]
        public async Task ApagarTodos_DeveRetornar204NoContent_SeTodosRemovidosComSucesso()
        {
            // Arrange
            _clienteServiceMock.Setup(service => service.ApagarTodosAsync())
                .ReturnsAsync(true);

            // Act
            var result = await _clienteController.ApagarTodos();

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
            _clienteServiceMock.Verify(service => service.ApagarTodosAsync(), Times.Once);
        }

        [Fact]
        public async Task ApagarTodos_DeveRetornar404NotFound_SeNenhumClienteRemovido()
        {
            // Arrange
            _clienteServiceMock.Setup(service => service.ApagarTodosAsync())
                .ReturnsAsync(false);

            // Act
            var result = await _clienteController.ApagarTodos();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            _clienteServiceMock.Verify(service => service.ApagarTodosAsync(), Times.Once);
        }
    }
}
