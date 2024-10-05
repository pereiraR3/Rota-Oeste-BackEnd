using System;
using api_rota_oeste.Controllers;
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using api_rota_oeste.Models.CheckList;

namespace api_rota_oeste.Tests.Controllers
{
    public class QuestaoControllerTest
    {
        private readonly Mock<IQuestaoService> _questaoServiceMock;
        private readonly QuestaoController _controller;

        public QuestaoControllerTest()
        {
            _questaoServiceMock = new Mock<IQuestaoService>();
            _controller = new QuestaoController(_questaoServiceMock.Object);
        }

        [Fact]
        public async Task Adicionar_DeveRetornarCreatedAtAction_QuandoSucesso()
        {
            // Arrange
            var questaoRequest = new QuestaoRequestDTO(1, "Titulo Teste", TipoQuestao.QUESTAO_OBJETIVA);
            var questaoResponse = new QuestaoResponseDTO(1, 1, "Titulo Teste", TipoQuestao.QUESTAO_OBJETIVA, null, null, null);

            _questaoServiceMock.Setup(service => service.AdicionarAsync(It.IsAny<QuestaoRequestDTO>()))
                .ReturnsAsync(questaoResponse);

            // Act
            var result = await _controller.Adicionar(questaoRequest);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            var retorno = Assert.IsType<QuestaoResponseDTO>(actionResult.Value);
            Assert.Equal(questaoResponse.Id, retorno.Id);
            _questaoServiceMock.Verify(service => service.AdicionarAsync(It.IsAny<QuestaoRequestDTO>()), Times.Once);
        }

        [Fact]
        public async Task BuscarTodos_DeveRetornarListaDeQuestoes()
        {
            // Arrange
            var checkListResponse = new CheckListResponseDTO
            {
                Id = 1,
                Nome = "CheckList Teste",
                Questoes = null,
                Usuario = null,
                UsuarioId = 1,
                DataCriacao = DateTime.Today
            };

            var questoes = new List<QuestaoResponseDTO>
            {
                new QuestaoResponseDTO(1, 1, "Titulo 1", TipoQuestao.QUESTAO_OBJETIVA, checkListResponse, null, null),
                new QuestaoResponseDTO(2, 1, "Titulo 2", TipoQuestao.QUESTAO_MULTIPLA_ESCOLHA, checkListResponse, null, null)
            };

            _questaoServiceMock.Setup(service => service.BuscarTodosAsync())
                .ReturnsAsync(questoes);

            // Act
            var result = await _controller.BuscarTodos();

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<QuestaoResponseDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<List<QuestaoResponseDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
            _questaoServiceMock.Verify(service => service.BuscarTodosAsync(), Times.Once);
        }

        [Fact]
        public async Task Obter_DeveRetornarQuestaoPorId()
        {
            // Arrange
            var checkListResponse = new CheckListResponseDTO
            {
                Id = 1,
                Nome = "CheckList Teste",
                Questoes = null,
                Usuario = null,
                UsuarioId = 1,
                DataCriacao = DateTime.Today
            };

            var questaoResponse = new QuestaoResponseDTO(1, 1, "Titulo Teste", TipoQuestao.QUESTAO_OBJETIVA, checkListResponse, null, null);

            _questaoServiceMock.Setup(service => service.BuscarPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(questaoResponse);

            // Act
            var result = await _controller.BuscarPorId(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<QuestaoResponseDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<QuestaoResponseDTO>(okResult.Value);
            Assert.Equal(questaoResponse, returnValue);
            _questaoServiceMock.Verify(service => service.BuscarPorIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task Obter_DeveRetornarNotFoundSeNaoEncontrada()
        {
            // Arrange
            _questaoServiceMock.Setup(service => service.BuscarPorIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new KeyNotFoundException("Questão não encontrada"));

            // Act
            var result = await _controller.BuscarPorId(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<QuestaoResponseDTO>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Questão não encontrada", notFoundResult.Value);
            _questaoServiceMock.Verify(service => service.BuscarPorIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task Atualizar_DeveRetornarNoContent_QuandoSucesso()
        {
            // Arrange
            var questaoPatch = new QuestaoPatchDTO(1, "Novo Titulo", TipoQuestao.QUESTAO_MULTIPLA_ESCOLHA);

            _questaoServiceMock.Setup(service => service.AtualizarAsync(questaoPatch))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Atualizar(questaoPatch);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _questaoServiceMock.Verify(service => service.AtualizarAsync(questaoPatch), Times.Once);
        }

        [Fact]
        public async Task Atualizar_DeveRetornarNotFound_QuandoQuestaoNaoExistir()
        {
            // Arrange
            var questaoPatch = new QuestaoPatchDTO(1, "Novo Titulo", TipoQuestao.QUESTAO_UPLOAD_DE_IMAGEM);

            _questaoServiceMock.Setup(service => service.AtualizarAsync(questaoPatch))
                .ThrowsAsync(new KeyNotFoundException("Questão não encontrada"));

            // Act
            var result = await _controller.Atualizar(questaoPatch);

            // Assert
            var actionResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Questão não encontrada", actionResult.Value);
            _questaoServiceMock.Verify(service => service.AtualizarAsync(questaoPatch), Times.Once);
        }

        [Fact]
        public async Task Apagar_DeveRetornarNoContent_QuandoSucesso()
        {
            // Arrange
            _questaoServiceMock.Setup(service => service.ApagarAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.ApagarPorId(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _questaoServiceMock.Verify(service => service.ApagarAsync(1), Times.Once);
        }

        [Fact]
        public async Task Apagar_DeveRetornarNotFound_QuandoQuestaoNaoExistir()
        {
            // Arrange
            _questaoServiceMock.Setup(service => service.ApagarAsync(1))
                .ThrowsAsync(new KeyNotFoundException("Questão não encontrada"));

            // Act
            var result = await _controller.ApagarPorId(1);

            // Assert
            var actionResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Questão não encontrada", actionResult.Value);
            _questaoServiceMock.Verify(service => service.ApagarAsync(1), Times.Once);
        }
    }
}
