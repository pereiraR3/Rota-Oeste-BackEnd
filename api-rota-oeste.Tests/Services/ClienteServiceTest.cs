using System;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services;
using api_rota_oeste.Services.Interfaces;
using AutoMapper;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Usuario;

namespace api_rota_oeste.Tests.Services
{
    public class ClienteServiceTest
    {
        private readonly Mock<IClienteRepository> _clienteRepositoryMock;
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ClienteService _clienteService;

        public ClienteServiceTest()
        {
            _clienteRepositoryMock = new Mock<IClienteRepository>();
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _mapperMock = new Mock<IMapper>();
            _clienteService = new ClienteService(_clienteRepositoryMock.Object, _mapperMock.Object, _usuarioRepositoryMock.Object);
        }

        [Fact]
        public async Task AdicionarAsync_DeveAdicionarCliente()
        {
            // Arrange
            var clienteRequest = new ClienteRequestDTO(1, "Nome", "123456789", new byte[] { });
    
            // Inicializando o usuarioModel corretamente com listas vazias
            var usuarioModel = new UsuarioModel
            {
                Id = 1, 
                Nome = "Test", 
                Senha = "12345678", 
                Telefone = "123456789", 
                Foto = new byte[] { },
                Clientes = new List<ClienteModel>(), // Inicializando como lista vazia
                CheckLists = new List<CheckListModel>() // Inicializando como lista vazia
            };
    
            var clienteModel = new ClienteModel(clienteRequest, usuarioModel);
            var clienteResponse = new ClienteResponseDTO(1, 1, "Nome", "123456789", null, usuarioModel);

            // Mock para buscar o usuário existente
            _usuarioRepositoryMock.Setup(repo => repo.BuscaPorId(clienteRequest.UsuarioId))
                .ReturnsAsync(usuarioModel);

            // Mock para adicionar o cliente
            _clienteRepositoryMock.Setup(repo => repo.Adicionar(It.IsAny<ClienteModel>()))
                .Callback<ClienteModel>(c => usuarioModel.Clientes.Add(c)) // Adiciona cliente ao usuário
                .ReturnsAsync(clienteModel); 

            // Mock para mapear o cliente para um DTO de resposta
            _mapperMock.Setup(mapper => mapper.Map<ClienteResponseDTO>(clienteModel))
                .Returns(clienteResponse);

            // Act
            var result = await _clienteService.AdicionarAsync(clienteRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(clienteResponse.Id, result.Id);
            Assert.Equal(clienteResponse.Nome, result.Nome);
            _usuarioRepositoryMock.Verify(repo => repo.BuscaPorId(clienteRequest.UsuarioId), Times.Once);
            _clienteRepositoryMock.Verify(repo => repo.Adicionar(It.IsAny<ClienteModel>()), Times.Once);
        }
        
        [Fact]
        public async Task AdicionarColecaoAsync_DeveAdicionarClientes()
        {
            // Arrange
            var clienteRequests = new List<ClienteRequestDTO>
            {
                new ClienteRequestDTO(1, "Nome", "123456789", new byte[] { }),
                new ClienteRequestDTO(1, "Nome2", "987654321", new byte[] { })
            };

            var clienteCollectionDto = new ClienteCollectionDTO(clienteRequests);
            var usuarioModel = new UsuarioModel();
            var clientesModel = clienteRequests.Select(req => new ClienteModel(req, usuarioModel)).ToList();
            var clientesResponse = clientesModel.Select(cliente => new ClienteResponseDTO(cliente.Id, cliente.UsuarioId, cliente.Nome, cliente.Telefone, cliente.Foto, usuarioModel)).ToList();

            _usuarioRepositoryMock.Setup(repo => repo.BuscaPorId(It.IsAny<int>()))
                .ReturnsAsync(usuarioModel);

            _mapperMock.Setup(mapper => mapper.Map<ClienteResponseDTO>(It.IsAny<ClienteModel>()))
                .Returns((ClienteModel source) => new ClienteResponseDTO(source.Id, source.UsuarioId, source.Nome, source.Telefone, source.Foto, source.Usuario));

            // Act
            var result = await _clienteService.AdicionarColecaoAsync(clienteCollectionDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            _clienteRepositoryMock.Verify(repo => repo.Adicionar(It.IsAny<ClienteModel>()), Times.Exactly(2));
        }

        [Fact]
        public async Task BuscarPorIdAsync_DeveRetornarCliente()
        {
            // Arrange
            var clienteModel = new ClienteModel { Id = 1, Nome = "Teste", Telefone = "123456789" };
            var clienteResponse = new ClienteResponseDTO(1, 1, "Teste", "123456789", null, null);

            _clienteRepositoryMock.Setup(repo => repo.BuscarPorId(1))
                .ReturnsAsync(clienteModel);

            _mapperMock.Setup(mapper => mapper.Map<ClienteResponseDTO>(clienteModel))
                .Returns(clienteResponse);

            // Act
            var result = await _clienteService.BuscarPorIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(clienteResponse.Id, result.Id);
            _clienteRepositoryMock.Verify(repo => repo.BuscarPorId(1), Times.Once);
        }

        [Fact]
        public async Task BuscarPorIdAsync_DeveLancarExcecao_SeNaoEncontrado()
        {
            // Arrange
            _clienteRepositoryMock.Setup(repo => repo.BuscarPorId(It.IsAny<int>()))
                .ReturnsAsync((ClienteModel)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _clienteService.BuscarPorIdAsync(1));
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
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _clienteService.ApagarAsync(1));
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
        public async Task BuscarTodosAsync_DeveRetornarTodosClientes()
        {
            // Arrange
            var clientesModel = new List<ClienteModel>
            {
                new ClienteModel { Id = 1, Nome = "Cliente 1", Telefone = "123456789" },
                new ClienteModel { Id = 2, Nome = "Cliente 2", Telefone = "987654321" }
            };

            _clienteRepositoryMock.Setup(repo => repo.BuscarTodos())
                .ReturnsAsync(clientesModel);

            _mapperMock.Setup(mapper => mapper.Map<ClienteResponseDTO>(It.IsAny<ClienteModel>()))
                .Returns((ClienteModel source) => new ClienteResponseDTO(source.Id, source.UsuarioId, source.Nome, source.Telefone, source.Foto, null));

            // Act
            var result = await _clienteService.BuscarTodosAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }
    }
}
