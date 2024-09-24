using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services.Interfaces;
using AutoMapper;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace api_rota_oeste.Tests.Services
{
    public class ClienteServiceTest
    {
        private readonly Mock<IClienteRepository> _clienteRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ClienteService _clienteService;

        public ClienteServiceTest()
        {
            _clienteRepositoryMock = new Mock<IClienteRepository>();
            _mapperMock = new Mock<IMapper>();
            _clienteService = new ClienteService(_clienteRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task AdicionarAsync_DeveAdicionarCliente()
        {
            // Arrange
            var clienteRequest = new ClienteRequestDTO(1, "Nome", "123456789", new byte[] { });
            var clienteModel = new ClienteModel { Id = 1, Nome = "Teste", Telefone = "123456789" , Foto = null};
            var clienteResponse = new ClienteResponseDTO(1, 1, "Teste", null, null);

            _clienteRepositoryMock.Setup(repo => repo.Adicionar(clienteRequest))
                .ReturnsAsync(clienteModel);

            _mapperMock.Setup(mapper => mapper.Map<ClienteResponseDTO>(clienteModel))
                .Returns(clienteResponse);

            // Act
            var result = await _clienteService.AdicionarAsync(clienteRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(clienteResponse.Id, result.Id);
            _clienteRepositoryMock.Verify(repo => repo.Adicionar(clienteRequest), Times.Once);
        }

        [Fact]
        public async Task AdicionarColecaoAsync_DeveAdicionarClientes()
        {
            // Arrange
            var clienteRequest = new ClienteCollectionDTO(new List<ClienteRequestDTO>
            {
                new ClienteRequestDTO(1, "Nome", "123456789", new byte[] { }),
                new ClienteRequestDTO(2, "Nome2", "1234567891", new byte[] { })
            });

            var clientesModel = new List<ClienteModel>
            {
                new ClienteModel { Id = 1, Nome = "Teste", Telefone = "123456789"},
                new ClienteModel { Id = 2, Nome = "Teste2", Telefone = "1234567891" }
            };

            _clienteRepositoryMock.Setup(repo => repo.AdicionarColecao(clienteRequest))
                .ReturnsAsync(clientesModel);

            var clientesResponse = clientesModel.Select(c => new ClienteResponseDTO (c.Id, c.UsuarioId, c.Nome, c.Telefone, c.Foto )).ToList();

            _mapperMock.Setup(mapper => mapper.Map<ClienteResponseDTO>(It.IsAny<ClienteModel>()))
                .Returns((ClienteModel source) => new ClienteResponseDTO ( source.Id, source.UsuarioId, source.Nome, source.Telefone, source.Foto ));

            // Act
            var result = await _clienteService.AdicionarColecaoAsync(clienteRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            _clienteRepositoryMock.Verify(repo => repo.AdicionarColecao(clienteRequest), Times.Once);
        }

        [Fact]
        public async Task BuscaPorIdAsync_DeveRetornarCliente()
        {
            // Arrange
            var clienteModel = new ClienteModel { Id = 1, Nome = "Teste", Telefone = "123456789", Foto = null };
            var clienteResponse = new ClienteResponseDTO ( 1, 1, "Teste", "123456789", null);

            _clienteRepositoryMock.Setup(repo => repo.BuscaPorId(1))
                .ReturnsAsync(clienteModel);

            _mapperMock.Setup(mapper => mapper.Map<ClienteResponseDTO>(clienteModel))
                .Returns(clienteResponse);

            // Act
            var result = await _clienteService.BuscaPorIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(clienteResponse.Id, result.Id);
            _clienteRepositoryMock.Verify(repo => repo.BuscaPorId(1), Times.Once);
        }

        [Fact]
        public async Task BuscaPorIdAsync_DeveLancarExcecao_SeNaoEncontrado()
        {
            // Arrange
            _clienteRepositoryMock.Setup(repo => repo.BuscaPorId(It.IsAny<int>()))
                .ReturnsAsync((ClienteModel)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _clienteService.BuscaPorIdAsync(1));
        }

        [Fact]
        public async Task ApagarAsync_DeveApagarCliente()
        {
            // Arrange
            _clienteRepositoryMock.Setup(repo => repo.Apagar(It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var result = await _clienteService.ApagarAsync(1);

            // Assert
            Assert.True(result);
            _clienteRepositoryMock.Verify(repo => repo.Apagar(1), Times.Once);
        }

        [Fact]
        public async Task ApagarAsync_DeveLancarExcecao_SeFalhar()
        {
            // Arrange
            _clienteRepositoryMock.Setup(repo => repo.Apagar(It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<ApplicationException>(() => _clienteService.ApagarAsync(1));
        }

        [Fact]
        public async Task ApagarTodosAsync_DeveApagarTodosClientes()
        {
            // Arrange
            _clienteRepositoryMock.Setup(repo => repo.ApagarTodos())
                .ReturnsAsync(true);

            // Act
            var result = await _clienteService.ApagarTodosAsync();

            // Assert
            Assert.True(result);
            _clienteRepositoryMock.Verify(repo => repo.ApagarTodos(), Times.Once);
        }

        [Fact]
        public async Task ApagarTodosAsync_DeveLancarExcecao_SeFalhar()
        {
            // Arrange
            _clienteRepositoryMock.Setup(repo => repo.ApagarTodos())
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<ApplicationException>(() => _clienteService.ApagarTodosAsync());
        }

        [Fact]
        public async Task BuscaTodosAsync_DeveRetornarTodosClientes()
        {
            // Arrange
            var clientesModel = new List<ClienteModel> 
            { 
                new ClienteModel { Id = 1 }, 
                new ClienteModel { Id = 2 } 
            };

            _clienteRepositoryMock.Setup(repo => repo.BuscaTodos())
                .ReturnsAsync(clientesModel);

            var clientesResponse = clientesModel.Select(c => new ClienteResponseDTO (c.Id, c.UsuarioId, c.Nome, c.Telefone, c.Foto)).ToList();

            _mapperMock.Setup(mapper => mapper.Map<ClienteResponseDTO>(It.IsAny<ClienteModel>()))
                .Returns((ClienteModel source) => new ClienteResponseDTO ( source.Id, source.UsuarioId, source.Nome, source.Telefone, source.Foto ));

            // Act
            var result = await _clienteService.BuscaTodosAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task BuscaTodosAsync_DeveLancarExcecao_SeNaoEncontrarClientes()
        {
            // Arrange
            _clienteRepositoryMock.Setup(repo => repo.BuscaTodos())
                .ReturnsAsync(new List<ClienteModel>());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _clienteService.BuscaTodosAsync());
        }
    }
}
