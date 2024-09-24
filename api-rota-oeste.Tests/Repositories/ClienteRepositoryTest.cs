using System;
using api_rota_oeste.Data;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.Usuario;
using api_rota_oeste.Repositories;
using api_rota_oeste.Repositories.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api_rota_oeste.Tests.Repositories
{
    public class ClienteRepositoryTest
    {
        private readonly ApiDBContext _dbContext;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly ClienteRepository _clienteRepository;

        public ClienteRepositoryTest()
        {
            // Configuração do DbContext para usar o InMemoryDatabase
            var options = new DbContextOptionsBuilder<ApiDBContext>()
                .UseInMemoryDatabase(databaseName: "ApiRotaOesteTestDB")
                .Options;

            _dbContext = new ApiDBContext(options);

            // Limpa os dados antes de cada teste
            _dbContext.Clientes.RemoveRange(_dbContext.Clientes);
            _dbContext.SaveChanges();

            // Mock para IMapper e IUsuarioRepository
            _mapperMock = new Mock<IMapper>();
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();

            // Instancia o repositório a ser testado
            _clienteRepository = new ClienteRepository(
                _mapperMock.Object, 
                _dbContext, 
                _usuarioRepositoryMock.Object);
        }
        
        [Fact]
        public async Task Adicionar_DeveAdicionarCliente()
        {
            // Arrange
            var clienteRequest = new ClienteRequestDTO(1, "Nome", "Endereço", new byte[] { });
            var usuario = new UsuarioModel { Id = 2, Nome = "Usuário Teste", Senha = "122123", Telefone = "123456789" };

            _usuarioRepositoryMock.Setup(repo => repo.BuscaPorId(It.IsAny<int>()))
                .ReturnsAsync(usuario);
        
            var clienteModel = new ClienteModel(clienteRequest, usuario);

            // Act
            var result = await _clienteRepository.Adicionar(clienteRequest);

            // Assert
            _usuarioRepositoryMock.Verify(repo => repo.BuscaPorId(It.IsAny<int>()), Times.Once);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task AdicionarColecao_DeveAdicionarClientes()
        {
            // Arrange
            var clienteRequest1 = new ClienteRequestDTO(1, "Nome", "Endereço", new byte[] { });
            var clienteRequest2 = new ClienteRequestDTO(2, "Outro Nome", "Outro Endereço", new byte[] { });
            var clienteCollection = new ClienteCollectionDTO(new List<ClienteRequestDTO> { clienteRequest1, clienteRequest2 });
            
            var usuario = new UsuarioModel { Id = 1, Nome = "Usuário Teste", Senha = "122123", Telefone = "123456789" };

            _usuarioRepositoryMock.Setup(repo => repo.BuscaPorId(It.IsAny<int>()))
                .ReturnsAsync(usuario);
            
            // Act
            var result = await _clienteRepository.AdicionarColecao(clienteCollection);

            // Assert
            _usuarioRepositoryMock.Verify(repo => repo.BuscaPorId(It.IsAny<int>()), Times.Once);
            Assert.Equal(2, result.Count);
        }
    
        [Fact]
        public async Task BuscaPorId_DeveRetornarCliente()
        {
            // Arrange
            var cliente = new ClienteModel { Id = 1, Nome = "Teste", Telefone = "123456789"};
            _dbContext.Clientes.Add(cliente);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _clienteRepository.BuscaPorId(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(cliente.Id, result.Id);
        }

        [Fact]
        public async Task BuscaPorId_DeveRetornarNullSeNaoEncontrado()
        {
            // Act
            var result = await _clienteRepository.BuscaPorId(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task BuscaTodos_DeveRetornarTodosClientes()
        {
            // Arrange
            var clientes = new List<ClienteModel> 
            { 
                new ClienteModel { Id = 1, Nome = "Teste", Telefone = "123456789"}, 
                new ClienteModel { Id = 2, Nome = "Teste", Telefone = "123456789"} 
            };
            _dbContext.Clientes.AddRange(clientes);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _clienteRepository.BuscaTodos();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task Apagar_DeveRemoverCliente()
        {
            // Arrange
            var cliente = new ClienteModel { Id = 1, Nome = "Teste", Telefone = "123456789"};
            _dbContext.Clientes.Add(cliente);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _clienteRepository.Apagar(1);

            // Assert
            Assert.True(result);
            Assert.DoesNotContain(cliente, _dbContext.Clientes);
        }

        [Fact]
        public async Task Apagar_DeveRetornarFalseSeNaoEncontrado()
        {
            // Act
            var result = await _clienteRepository.Apagar(1);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ApagarTodos_DeveRemoverTodosClientes()
        {
            // Arrange
            var clientes = new List<ClienteModel>
            {
                new ClienteModel { Id = 1, Nome = "Teste", Telefone = "123456789"},
                new ClienteModel { Id = 2, Nome = "Teste", Telefone = "123456789"}
            };
            _dbContext.Clientes.AddRange(clientes);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _clienteRepository.ApagarTodos();

            // Assert
            Assert.True(result);
            Assert.Empty(_dbContext.Clientes);
        }
    }
}
