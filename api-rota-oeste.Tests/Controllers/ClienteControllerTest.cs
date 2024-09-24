using api_rota_oeste.Controllers;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api_rota_oeste.Tests.Controllers
{
    public class ClienteControllerTest
    {
        private readonly ClienteController _controller;
        private readonly Mock<IClienteService> _clienteServiceMock;

        public ClienteControllerTest()
        {
            _clienteServiceMock = new Mock<IClienteService>();
            _controller = new ClienteController(_clienteServiceMock.Object);
        }

        [Fact]
        public async Task Adicionar_DeveRetornarCreatedAtAction_QuandoClienteAdicionado()
        {
            // Arrange
            var clienteRequest = new ClienteRequestDTO(1, "Teste", "123456789", new byte[0]);
            var clienteResponse = new ClienteResponseDTO(1, 1, "Teste", "123456789", new byte[0]);
            
            _clienteServiceMock
                .Setup(s => s.AdicionarAsync(clienteRequest))
                .ReturnsAsync(clienteResponse);

            // Act
            var result = await _controller.Adicionar(clienteRequest);
            
            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("BuscarPorId", actionResult.ActionName);
            Assert.Equal(clienteResponse, actionResult.Value);
        }

        [Fact]
        public async Task AdicionarColecao_DeveRetornarOk_QuandoClientesAdicionados()
        {
            // Arrange
            var clienteCollection = new ClienteCollectionDTO(new List<ClienteRequestDTO>
            {
                new ClienteRequestDTO(1, "Teste1", "123456789", new byte[0]),
                new ClienteRequestDTO(2, "Teste2", "987654321", new byte[0])
            });
            
            var clienteResponseList = new List<ClienteResponseDTO>
            {
                new ClienteResponseDTO(1, 1, "Teste1", "123456789", new byte[0]),
                new ClienteResponseDTO(2, 2, "Teste2", "987654321", new byte[0])
            };
            
            _clienteServiceMock
                .Setup(s => s.AdicionarColecaoAsync(clienteCollection))
                .ReturnsAsync(clienteResponseList);

            // Act
            var result = await _controller.AdicionarColecao(clienteCollection);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(clienteResponseList, actionResult.Value);
        }

        [Fact]
        public async Task BuscarPorId_DeveRetornarOk_QuandoClienteEncontrado()
        {
            // Arrange
            var clienteResponse = new ClienteResponseDTO(1, 1, "Teste", "123456789", new byte[0]);
            
            _clienteServiceMock
                .Setup(s => s.BuscaPorIdAsync(1))
                .ReturnsAsync(clienteResponse);

            // Act
            var result = await _controller.BuscarPorId(1);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(clienteResponse, actionResult.Value);
        }

        [Fact]
        public async Task BuscarPorId_DeveRetornarNotFound_QuandoClienteNaoEncontrado()
        {
            // Arrange
            _clienteServiceMock
                .Setup(s => s.BuscaPorIdAsync(1))
                .ReturnsAsync((ClienteResponseDTO)null);

            // Act
            var result = await _controller.BuscarPorId(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Apagar_DeveRetornarNoContent_QuandoClienteRemovidoComSucesso()
        {
            // Arrange
            _clienteServiceMock
                .Setup(s => s.ApagarAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Apagar(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Apagar_DeveRetornarNotFound_QuandoClienteNaoEncontrado()
        {
            // Arrange
            _clienteServiceMock
                .Setup(s => s.ApagarAsync(1))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Apagar(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task BuscarTodos_DeveRetornarOk_QuandoClientesExistem()
        {
            // Arrange
            var clientes = new List<ClienteResponseDTO>
            {
                new ClienteResponseDTO(1, 1, "Teste1", "123456789", new byte[0]),
                new ClienteResponseDTO(2, 2, "Teste2", "987654321", new byte[0])
            };
            
            _clienteServiceMock
                .Setup(s => s.BuscaTodosAsync())
                .ReturnsAsync(clientes);

            // Act
            var result = await _controller.BuscarTodos();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(clientes, actionResult.Value);
        }

        [Fact]
        public async Task BuscarTodos_DeveRetornarNoContent_QuandoNenhumClienteEncontrado()
        {
            // Arrange
            _clienteServiceMock
                .Setup(s => s.BuscaTodosAsync())
                .ReturnsAsync((List<ClienteResponseDTO>)null);

            // Act
            var result = await _controller.BuscarTodos();

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }
    }
}
