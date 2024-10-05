using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Repositories;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services;
using AutoMapper;
using Moq;
using Xunit;

namespace api_rota_oeste.Tests.Services
{
    public class QuestaoServiceTest
    {
        private readonly Mock<IQuestaoRepository> _questaoRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICheckListRepository> _repositoryCheckListMock;
        private readonly Mock<IRepository> _repositoryMock;
        private readonly QuestaoService _questaoService;

        public QuestaoServiceTest()
        {
            _repositoryCheckListMock = new Mock<ICheckListRepository>();
            _questaoRepositoryMock = new Mock<IQuestaoRepository>();
            _mapperMock = new Mock<IMapper>();
            _repositoryMock = new Mock<IRepository>();
        
            _questaoService = new QuestaoService(
                _questaoRepositoryMock.Object,
                _mapperMock.Object,
                _repositoryCheckListMock.Object,
                _repositoryMock.Object
            );
        }

        [Fact]
        public async Task AdicionarAsync_DeveRetornarQuestaoResponseDTO()
        {
            // Arrange
            var questaoRequest = new QuestaoRequestDTO(1, "Titulo Teste", TipoQuestao.QUESTAO_OBJETIVA);
   
            var checkListModel = new CheckListModel
            {
                Id = 1,
                Nome = "CheckList Teste",
                Questoes = new List<QuestaoModel>(),
                Usuario = null,
                UsuarioId = 1,
                DataCriacao = DateTime.Today
            };

            var questaoModel = new QuestaoModel
            {
                Id = 1,
                Titulo = "Titulo Teste",
                Tipo = TipoQuestao.QUESTAO_OBJETIVA,
                CheckList = checkListModel
            };
            
            var checkListResponse = _mapperMock.Object.Map<CheckListResponseDTO>(checkListModel);

            var questaoResponse = new QuestaoResponseDTO(1, 1, "Titulo Teste", TipoQuestao.QUESTAO_OBJETIVA, checkListResponse, null, null);
    
            // Configurando o mock para buscar o CheckList
            _repositoryCheckListMock.Setup(repo => repo.BuscarPorId(questaoRequest.CheckListId))
                .ReturnsAsync(checkListModel);
            
            _questaoRepositoryMock.Setup(repo => repo.Adicionar(It.IsAny<QuestaoModel>()))
                .ReturnsAsync(questaoModel);
            
            _mapperMock.Setup(mapper => mapper.Map<QuestaoResponseDTO>(questaoModel))
                .Returns(questaoResponse);

            // Act
            var result = await _questaoService.AdicionarAsync(questaoRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Titulo Teste", result.Titulo);
            Assert.Equal(TipoQuestao.QUESTAO_OBJETIVA, result.Tipo);
        }

        [Fact]
        public async Task BuscarPorIdAsync_DeveRetornarQuestaoResponseDTO_SeEncontrado()
        {
            // Arrange
            CheckListResponseDTO checkList = new CheckListResponseDTO
            {
                Id = 1,
                Nome = "CheckList Teste",
                Questoes = null,
                Usuario = null,
                UsuarioId = 1,
                DataCriacao = DateTime.Today
            };

            var questaoModel = new QuestaoModel { Id = 1, Titulo = "Titulo Teste", Tipo = TipoQuestao.QUESTAO_OBJETIVA };
            var questaoResponse = new QuestaoResponseDTO(1, 1, "Titulo Teste", TipoQuestao.QUESTAO_OBJETIVA, checkList, null, null);

            _questaoRepositoryMock.Setup(repo => repo.BuscarPorId(It.IsAny<int>()))
                .ReturnsAsync(questaoModel);
            _mapperMock.Setup(mapper => mapper.Map<QuestaoResponseDTO>(questaoModel))
                .Returns(questaoResponse);

            // Act
            var result = await _questaoService.BuscarPorIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Titulo Teste", result.Titulo);
            Assert.Equal(TipoQuestao.QUESTAO_OBJETIVA, result.Tipo);
        }

        [Fact]
        public async Task BuscarPorIdAsync_DeveLancarExcecao_SeNaoEncontrado()
        {
            // Arrange
            _questaoRepositoryMock.Setup(repo => repo.BuscarPorId(It.IsAny<int>()))
                .ReturnsAsync((QuestaoModel)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _questaoService.BuscarPorIdAsync(1));
        }

        [Fact]
        public async Task AtualizarAsync_DeveRetornarTrue_SeAtualizacaoBemSucedida()
        {
            // Arrange
            var questaoPatch = new QuestaoPatchDTO(1, "Novo Titulo", TipoQuestao.QUESTAO_MULTIPLA_ESCOLHA);
            var questaoModel = new QuestaoModel { Id = 1, Titulo = "Titulo Antigo", Tipo = TipoQuestao.QUESTAO_OBJETIVA };

            _questaoRepositoryMock.Setup(repo => repo.BuscarPorId(questaoPatch.Id))
                .ReturnsAsync(questaoModel);
            _mapperMock.Setup(mapper => mapper.Map(questaoPatch, questaoModel))
                .Verifiable();

            // Act
            var result = await _questaoService.AtualizarAsync(questaoPatch);

            // Assert
            Assert.True(result);
            _mapperMock.Verify(mapper => mapper.Map(questaoPatch, questaoModel), Times.Once);
        }

        [Fact]
        public async Task AtualizarAsync_DeveLancarExcecao_SeQuestaoNaoEncontrada()
        {
            // Arrange
            var questaoPatch = new QuestaoPatchDTO(1, "Novo Titulo", TipoQuestao.QUESTAO_UPLOAD_DE_IMAGEM);

            _questaoRepositoryMock.Setup(repo => repo.BuscarPorId(questaoPatch.Id))
                .ReturnsAsync((QuestaoModel)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _questaoService.AtualizarAsync(questaoPatch));
        }

        [Fact]
        public async Task ApagarAsync_DeveRetornarTrue_SeApagarBemSucedido()
        {
            // Arrange
            var questaoModel = new QuestaoModel { Id = 1, Titulo = "Titulo Teste", Tipo = TipoQuestao.QUESTAO_OBJETIVA };

            _questaoRepositoryMock.Setup(repo => repo.BuscarPorId(It.IsAny<int>()))
                .ReturnsAsync(questaoModel);
    
            _questaoRepositoryMock.Setup(repo => repo.Apagar(It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var result = await _questaoService.ApagarAsync(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ApagarAsync_DeveLancarExcecao_SeQuestaoNaoEncontrada()
        {
            // Arrange
            _questaoRepositoryMock.Setup(repo => repo.BuscarPorId(1))
                .ReturnsAsync((QuestaoModel)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _questaoService.ApagarAsync(1));
        }
    }
}
