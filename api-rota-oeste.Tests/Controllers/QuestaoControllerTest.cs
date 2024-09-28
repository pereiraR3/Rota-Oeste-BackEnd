using api_rota_oeste.Controllers;
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Collections.Generic;
using api_rota_oeste.Repositories;
using api_rota_oeste.Services.Interfaces;
using AutoMapper;

namespace api_rota_oeste.Tests.Controllers
{
    public class QuestaoControllerTest
    {
        private readonly Mock<IQuestaoRepository> _questaoRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IQuestaoService _questaoService;
        private readonly QuestaoController _controller;

        public QuestaoControllerTest()
        {
            _questaoRepositoryMock = new Mock<IQuestaoRepository>();
            _mapperMock = new Mock<IMapper>();
            _questaoService = new QuestaoService(_questaoRepositoryMock.Object, _mapperMock.Object);
            _controller = new QuestaoController(_questaoService);
        }

        [Fact]
        public void Criar_DeveRetornarCreated()
        {
            // Arrange
            var questaoDto = new QuestaoRequestDTO("tituloteste", "tipoteste");
            var questaoModel = new QuestaoModel("tituloteste", "tipoteste");

            _mapperMock.Setup(m => m.Map<QuestaoModel>(questaoDto)).Returns(questaoModel);

            // Act
            var result = _controller.criar(questaoDto);

            // Assert
            var actionResult = Assert.IsType<OkResult>(result);
            _questaoRepositoryMock.Verify(repo => repo.criar(questaoModel), Times.Once);
        }

        [Fact]
        public void Listar_DeveRetornarListaDeQuestoes()
        {
            // Arrange
            var questoes = new List<QuestaoModel> 
            { 
                new QuestaoModel("Titulo 1", "Tipo 1"), 
                new QuestaoModel("Titulo 2", "Tipo 2") 
            };
            _questaoRepositoryMock.Setup(s => s.listar()).Returns(questoes);

            // Act
            var result = _controller.Listar();

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<QuestaoModel>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result); // Extrai o OkObjectResult do ActionResult
            var returnValue = Assert.IsType<List<QuestaoModel>>(okResult.Value); // Verifica o valor retornado
            Assert.Equal(2, returnValue.Count); // Verifica se a lista contém o número esperado de itens
            _questaoRepositoryMock.Verify(s => s.listar(), Times.Once);
        }


        [Fact]
        public void Obter_DeveRetornarQuestaoPorId()
        {
            // Arrange
            var questaoModel = new QuestaoModel("tituloteste", "tipoteste");
            var questaoResponseDto = new QuestaoResponseDTO("tituloteste", "tipoteste");

            _questaoRepositoryMock.Setup(repo => repo.obter(1)).Returns(questaoModel);
            _mapperMock.Setup(m => m.Map<QuestaoResponseDTO>(questaoModel)).Returns(questaoResponseDto);

            // Act
            var result = _controller.Obter(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<QuestaoResponseDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result); // Verifica se o resultado é um OkObjectResult
            var returnValue = Assert.IsType<QuestaoResponseDTO>(okResult.Value); // Verifica se o valor retornado é do tipo QuestaoResponseDTO
            Assert.Equal(questaoResponseDto, returnValue);
            _questaoRepositoryMock.Verify(repo => repo.obter(1), Times.Once);
        }


        [Fact]
        public void Atualizar_DeveRetornarNoContent()
        {
            // Arrange
            var questaoDto = new QuestaoRequestDTO("tituloteste", "tipoteste");
            var questaoModel = new QuestaoModel("tituloteste", "tipoteste");

            _questaoRepositoryMock.Setup(repo => repo.obter(1)).Returns(questaoModel);

            // Act
            var result = _controller.Atualizar(1, questaoDto);

            // Assert
            var actionResult = Assert.IsType<NoContentResult>(result);
            _questaoRepositoryMock.Verify(repo => repo.salvar(), Times.Once);
        }

        [Fact]
        public void Excluir_DeveRetornarNoContent()
        {
            // Arrange
            var questaoModel = new QuestaoModel("tituloteste", "tipoteste");

            _questaoRepositoryMock.Setup(repo => repo.obter(1)).Returns(questaoModel);

            // Act
            var result = _controller.Excluir(1);

            // Assert
            var actionResult = Assert.IsType<NoContentResult>(result);
            _questaoRepositoryMock.Verify(repo => repo.deletar(1), Times.Once);
        }

        [Fact]
        public void Obter_QuandoQuestaoNaoExiste_DeveLancarKeyNotFoundException()
        {
            // Arrange
            _questaoRepositoryMock.Setup(repo => repo.obter(1)).Returns((QuestaoModel)null);

            // Act & Assert
            var exception = Assert.Throws<KeyNotFoundException>(() => _controller.Obter(1));

            // Verifica se a mensagem da exceção é a esperada
            Assert.Equal("Não há questão registrada com o ID informado.", exception.Message);

            _questaoRepositoryMock.Verify(repo => repo.obter(1), Times.Once);
        }

    }
}
