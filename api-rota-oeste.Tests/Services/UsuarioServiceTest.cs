using System;
using System.Collections.Generic;
using api_rota_oeste.Models.Usuario;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services;
using AutoMapper;
using Moq;
using Xunit;
using System.Threading.Tasks;

namespace api_rota_oeste.Tests.Services
{
    public class UsuarioServiceTest
    {
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly Mock<IRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UsuarioService _usuarioService;

        public UsuarioServiceTest()
        {
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _repositoryMock = new Mock<IRepository>();
            _mapperMock = new Mock<IMapper>();
            _usuarioService = new UsuarioService(_usuarioRepositoryMock.Object, _mapperMock.Object, _repositoryMock.Object);
        }

        // Teste para AdicionarAsync
        [Fact]
        public async Task AdicionarAsync_DeveRetornarUsuarioResponseDTO()
        {
            // Arrange
            var usuarioRequest = new UsuarioRequestDTO
            (
                "66992337652",
                "Teste",
                "12345",
                null
            );
            
            var usuarioModel = new UsuarioModel
            {
                Id = 1,
                Nome = "Teste",
                Telefone = "66992337652",
                Senha = "12345"
            };

            var usuarioResponse = new UsuarioResponseDTO
            (
                1,
                "66992337652",
                "Teste",
                null,
                null,
                null
            );

            _usuarioRepositoryMock.Setup(repo => repo.Adicionar(It.IsAny<UsuarioModel>()))
                .ReturnsAsync(usuarioModel);

            _mapperMock.Setup(mapper => mapper.Map<UsuarioResponseDTO>(usuarioModel))
                .Returns(usuarioResponse);

            // Act
            var result = await _usuarioService.AdicionarAsync(usuarioRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(usuarioResponse.Id, result.Id);
            Assert.Equal(usuarioResponse.Nome, result.Nome);
            Assert.Equal(usuarioResponse.Telefone, result.Telefone);
        }

        // Teste para BuscaPorIdAsync - Usuário encontrado
        [Fact]
        public async Task BuscaPorIdAsync_DeveRetornarUsuarioResponseDTO_SeUsuarioEncontrado()
        {
            // Arrange
            var usuarioModel = new UsuarioModel
            {
                Id = 1,
                Nome = "Teste",
                Telefone = "66992337652",
                Senha = "12345"
            };

            var usuarioResponse = new UsuarioResponseDTO
            (
                1,
                "66992337652",
                "Teste",
                null,
                null,
                null
            );

            _usuarioRepositoryMock.Setup(repo => repo.BuscaPorId(It.IsAny<int>()))
                .ReturnsAsync(usuarioModel);

            _mapperMock.Setup(mapper => mapper.Map<UsuarioResponseDTO>(usuarioModel))
                .Returns(usuarioResponse);

            // Act
            var result = await _usuarioService.BuscaPorIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(usuarioResponse.Id, result.Id);
            Assert.Equal(usuarioResponse.Nome, result.Nome);
        }

        // Teste para BuscaPorIdAsync - Usuário não encontrado
        [Fact]
        public async Task BuscaPorIdAsync_DeveLancarExcecao_SeUsuarioNaoEncontrado()
        {
            // Arrange
            _usuarioRepositoryMock.Setup(repo => repo.BuscaPorId(It.IsAny<int>()))
                .ReturnsAsync((UsuarioModel)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _usuarioService.BuscaPorIdAsync(1));
        }

        // Teste para AtualizarAsync - Sucesso
        [Fact]
        public async Task AtualizarAsync_DeveRetornarTrue_SeAtualizacaoBemSucedida()
        {
            // Arrange
            var usuarioPatch = new UsuarioPatchDTO
            {
                Id = 1,
                Nome = "Novo Nome",
                Telefone = "66992337652"
            };

            var usuarioModel = new UsuarioModel
            {
                Id = 1,
                Nome = "Teste",
                Telefone = "66992337652",
                Senha = "12345"
            };

            _usuarioRepositoryMock.Setup(repo => repo.BuscaPorId(usuarioPatch.Id))
                .ReturnsAsync(usuarioModel);

            _mapperMock.Setup(mapper => mapper.Map(usuarioPatch, usuarioModel))
                .Verifiable();

            _repositoryMock.Setup(repo => repo.Salvar())
                .Verifiable();

            // Act
            var result = await _usuarioService.AtualizarAsync(usuarioPatch);

            // Assert
            Assert.True(result);
        }

        // Teste para AtualizarAsync - Falha
        [Fact]
        public async Task AtualizarAsync_DeveRetornarFalse_SeUsuarioNaoEncontrado()
        {
            // Arrange
            var usuarioPatch = new UsuarioPatchDTO
            {
                Id = 1,
                Nome = "Novo Nome",
                Telefone = "66992337652"
            };

            _usuarioRepositoryMock.Setup(repo => repo.BuscaPorId(usuarioPatch.Id))
                .ReturnsAsync((UsuarioModel)null);

            // Act
            var result = await _usuarioService.AtualizarAsync(usuarioPatch);

            // Assert
            Assert.False(result);
        }

        // Teste para ApagarAsync - Sucesso
        [Fact]
        public async Task ApagarAsync_DeveRetornarTrue_SeUsuarioApagadoComSucesso()
        {
            // Arrange
            _usuarioRepositoryMock.Setup(repo => repo.Apagar(It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var result = await _usuarioService.ApagarAsync(1);

            // Assert
            Assert.True(result);
        }

        // Teste para ApagarAsync - Usuário não encontrado
        [Fact]
        public async Task ApagarAsync_DeveLancarExcecao_SeUsuarioNaoEncontrado()
        {
            // Arrange
            _usuarioRepositoryMock.Setup(repo => repo.Apagar(It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _usuarioService.ApagarAsync(1));
        }
    }
}
