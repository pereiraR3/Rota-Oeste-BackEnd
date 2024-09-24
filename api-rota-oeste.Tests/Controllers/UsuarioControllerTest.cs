using System.Threading.Tasks;
using api_rota_oeste.Controllers;
using api_rota_oeste.Models.Usuario;
using api_rota_oeste.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace api_rota_oeste.Tests.Controllers
{
    public class UsuarioControllerTest
    {
        private readonly UsuarioController _controller;
        private readonly Mock<IUsuarioService> _usuarioServiceMock;

        public UsuarioControllerTest()
        {
            // Criando os mocks necessários
            _usuarioServiceMock = new Mock<IUsuarioService>();
    
            // Inicializando o controller com o mock do repositório
            _controller = new UsuarioController(_usuarioServiceMock.Object);
        }


        // Teste para o método Adicionar
        [Fact]
        public async Task Adicionar_DeveRetornar201Created()
        {
            // Arrange
            var usuarioRequest = new UsuarioRequestDTO
            (
                "66992337652",
                "Teste",
                "12345",
                null
            );

            var usuarioResponse = new UsuarioResponseDTO
            (
                1,
                "66992337652",
                "Teste",
                null
            );

            _usuarioServiceMock.Setup(repo => repo.AdicionarAsync(It.IsAny<UsuarioRequestDTO>()))
                .ReturnsAsync(usuarioResponse);

            // Act
            var result = await _controller.Adicionar(usuarioRequest);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result); // Valida o tipo correto
            Assert.NotNull(createdAtActionResult);
            Assert.Equal(201, createdAtActionResult.StatusCode);
            Assert.Equal(usuarioResponse, createdAtActionResult.Value);
        }

        // Teste para o método ObterPorId
        [Fact]
        public async Task ObterPorId_DeveRetornar200ComUsuario()
        {
            // Arrange
            var usuarioModel = new UsuarioResponseDTO
            (
                1,
                "66992337652",
                "Teste",
                null
            );

            _usuarioServiceMock.Setup(repo => repo.BuscaPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(usuarioModel);

            // Act
            var result = await _controller.ObterPorId(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result); // Verifica se o tipo retornado é OkObjectResult
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(usuarioModel, okResult.Value);
        }


        [Fact]
        public async Task ObterPorId_DeveRetornar404SeNaoEncontrado()
        {
            // Arrange
            _usuarioServiceMock.Setup(repo => repo.BuscaPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync((UsuarioResponseDTO) null);

            // Act
            var result = await _controller.ObterPorId(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        // Teste para o método Atualizar
        [Fact]
        public async Task Atualizar_DeveRetornar204NoContent()
        {
            // Arrange
            var usuarioPatch = new UsuarioPatchDTO
            {
                Id = 1,
                Nome = "Novo Nome",
                Telefone = "66992337652"
            };

            _usuarioServiceMock.Setup(repo => repo.AtualizarAsync(It.IsAny<UsuarioPatchDTO>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Atualizar(usuarioPatch);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Atualizar_DeveRetornar404SeNaoEncontrado()
        {
            // Arrange
            var usuarioPatch = new UsuarioPatchDTO
            {
                Id = 1,
                Nome = "Novo Nome",
                Telefone = "66992337652"
            };

            _usuarioServiceMock.Setup(repo => repo.AtualizarAsync(It.IsAny<UsuarioPatchDTO>()))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Atualizar(usuarioPatch);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // Teste para o método Apagar
        [Fact]
        public async Task Apagar_DeveRetornar204NoContent()
        {
            // Arrange
            _usuarioServiceMock.Setup(repo => repo.ApagarAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Apagar(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Apagar_DeveRetornar404SeNaoEncontrado()
        {
            // Arrange
            _usuarioServiceMock.Setup(repo => repo.ApagarAsync(It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Apagar(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
