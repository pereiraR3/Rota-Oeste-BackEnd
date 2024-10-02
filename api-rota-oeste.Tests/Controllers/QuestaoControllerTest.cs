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
        public async Task Adicionar_DeveRetornarOk()
        {
            // Arrange
            var questaoRequest = new QuestaoRequestDTO(1, "Titulo Teste", "Tipo Teste");

            _questaoServiceMock.Setup(service => service.AdicionarAsync(It.IsAny<QuestaoRequestDTO>()))
                .ReturnsAsync(new QuestaoResponseDTO(1, 1,"Titulo Teste", "Tipo Teste", null, null));

            // Act
            var result = await _controller.Adicionar(questaoRequest);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result);
            _questaoServiceMock.Verify(service => service.AdicionarAsync(It.IsAny<QuestaoRequestDTO>()), Times.Once);
        }

        [Fact]
        public async Task BuscarTodos_DeveRetornarListaDeQuestoes()
        {
            // Arrange
            CheckListModel checkListModel = new CheckListModel
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
                new QuestaoResponseDTO(1, 1,"Titulo 1", "Tipo 1", checkListModel, null),
                new QuestaoResponseDTO(2, 1,"Titulo 2", "Tipo 2", checkListModel, null)
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
            CheckListModel checkListModel = new CheckListModel
            {
                Id = 1,
                Nome = "CheckList Teste",
                Questoes = null,
                Usuario = null,
                UsuarioId = 1,
                DataCriacao = DateTime.Today
            };

            var questaoResponse = new QuestaoResponseDTO(1, 1, "Titulo Teste", "Tipo Teste", checkListModel, null);

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
                .ReturnsAsync((QuestaoResponseDTO)null);

            // Act
            var result = await _controller.BuscarPorId(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<QuestaoResponseDTO>>(result);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            _questaoServiceMock.Verify(service => service.BuscarPorIdAsync(1), Times.Once);
        }

        // Outros testes corrigidos de forma semelhante...
    }
}
