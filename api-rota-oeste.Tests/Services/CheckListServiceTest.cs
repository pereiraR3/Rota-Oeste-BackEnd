using System;
using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services;
using api_rota_oeste.Models.Usuario;
using AutoMapper;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_rota_oeste.Models.Cliente;
using Microsoft.Extensions.Logging;

namespace api_rota_oeste.Tests.Services
{
    public class CheckListServiceTest
    {
        private readonly Mock<ICheckListRepository> _repositoryCheckListMock;
        private readonly Mock<IUsuarioRepository> _repositoryUsuarioMock;
        private readonly Mock<IRepository> _repositoryMock;
        private readonly Mock<IClienteRespondeCheckListRepository> _repositoryClienteRespondeCheckMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IClienteRepository> _repositoryClienteMock;
        private readonly Mock<ILogger<CheckListService>> _loggerMock;
        private readonly Mock<IInteracaoRepository> _repositoryInteracaoMock;
        private readonly CheckListService _checkListService;

        public CheckListServiceTest()
        {
            _repositoryCheckListMock = new Mock<ICheckListRepository>();
            _repositoryUsuarioMock = new Mock<IUsuarioRepository>();
            _repositoryMock = new Mock<IRepository>();
            _mapperMock = new Mock<IMapper>();
            _repositoryClienteRespondeCheckMock = new Mock<IClienteRespondeCheckListRepository>();
            _repositoryClienteMock = new Mock<IClienteRepository>();
            _loggerMock = new Mock<ILogger<CheckListService>>();
            _repositoryInteracaoMock = new Mock<IInteracaoRepository>();

            _checkListService = new CheckListService(
                _repositoryCheckListMock.Object,
                _repositoryUsuarioMock.Object,
                _mapperMock.Object,
                _repositoryMock.Object,
                _repositoryClienteRespondeCheckMock.Object,
                _repositoryClienteMock.Object,
                _repositoryInteracaoMock.Object,
                _loggerMock.Object
            );
        }
        
        [Fact]
        public async Task AdicionarAsync_DeveAdicionarCheckList()
        {
            // Arrange
            var checkListRequest = new CheckListRequestDTO(1, "Checklist Teste");
            var usuarioModel = new UsuarioModel
            {
                Id = 1,
                Nome = "Usuario Teste",
                Clientes = new List<ClienteModel>(),
                CheckLists = new List<CheckListModel>()
            };
            var checkListModel = new CheckListModel(checkListRequest, usuarioModel);
            var checkListResponse = new CheckListResponseDTO(1, 1, "Checklist Teste", DateTime.Now, null, null, null);

            _repositoryUsuarioMock.Setup(repo => repo.BuscaPorId(checkListRequest.UsuarioId))
                .ReturnsAsync(usuarioModel);

            _repositoryCheckListMock.Setup(repo => repo.Adicionar(It.IsAny<CheckListModel>()))
                .ReturnsAsync(checkListModel);

            _mapperMock.Setup(mapper => mapper.Map<CheckListResponseDTO>(checkListModel))
                .Returns(checkListResponse);

            // Act
            var result = await _checkListService.AdicionarAsync(checkListRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(checkListResponse.Id, result.Id);
            Assert.Equal(checkListResponse.Nome, result.Nome);
            _repositoryUsuarioMock.Verify(repo => repo.BuscaPorId(checkListRequest.UsuarioId), Times.Once);
            _repositoryCheckListMock.Verify(repo => repo.Adicionar(It.IsAny<CheckListModel>()), Times.Once);
        }

        [Fact]
        public async Task BuscarPorIdAsync_DeveRetornarCheckList()
        {
            // Arrange
            var checkListModel = new CheckListModel
            {
                Id = 1,
                Nome = "Checklist Teste",
                UsuarioId = 1
            };
            var checkListResponse = new CheckListResponseDTO(1, 1, "Checklist Teste", DateTime.Now, null, null, null);

            _repositoryCheckListMock.Setup(repo => repo.BuscarPorId(1))
                .ReturnsAsync(checkListModel);

            _mapperMock.Setup(mapper => mapper.Map<CheckListResponseDTO>(checkListModel))
                .Returns(checkListResponse);

            // Act
            var result = await _checkListService.BuscarPorIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(checkListResponse.Id, result.Id);
            _repositoryCheckListMock.Verify(repo => repo.BuscarPorId(1), Times.Once);
        }

        [Fact]
        public async Task BuscarPorIdAsync_DeveLancarExcecao_SeNaoEncontrado()
        {
            // Arrange
            _repositoryCheckListMock.Setup(repo => repo.BuscarPorId(It.IsAny<int>()))
                .ReturnsAsync((CheckListModel)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _checkListService.BuscarPorIdAsync(1));
        }

        [Fact]
        public async Task BuscarTodosAsync_DeveRetornarTodosCheckLists()
        {
            // Arrange
            var checkLists = new List<CheckListModel>
            {
                new CheckListModel { Id = 1, Nome = "Checklist 1" },
                new CheckListModel { Id = 2, Nome = "Checklist 2" }
            };
            var checkListsResponse = checkLists.Select(c => new CheckListResponseDTO(c.Id, c.UsuarioId, c.Nome, DateTime.Now, null, null, null)).ToList();

            _repositoryCheckListMock.Setup(repo => repo.BuscarTodos())
                .ReturnsAsync(checkLists);

            _mapperMock.Setup(mapper => mapper.Map<CheckListResponseDTO>(It.IsAny<CheckListModel>()))
                .Returns((CheckListModel source) => new CheckListResponseDTO(source.Id, source.UsuarioId, source.Nome, source.DataCriacao, null, null, null));

            // Act
            var result = await _checkListService.BuscarTodosAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            _repositoryCheckListMock.Verify(repo => repo.BuscarTodos(), Times.Once);
        }

        [Fact]
        public async Task AtualizarAsync_DeveAtualizarCheckList()
        {
            // Arrange
            var checkListPatchDto = new CheckListPatchDTO(1, "Novo Nome");
            var checkListModel = new CheckListModel
            {
                Id = 1,
                Nome = "Checklist Antigo",
                UsuarioId = 1
            };

            _repositoryCheckListMock.Setup(repo => repo.BuscarPorId(checkListPatchDto.Id))
                .ReturnsAsync(checkListModel);

            _mapperMock.Setup(mapper => mapper.Map(checkListPatchDto, checkListModel))
                .Verifiable();

            // Act
            var result = await _checkListService.AtualizarAsync(checkListPatchDto);

            // Assert
            Assert.True(result);
            _repositoryCheckListMock.Verify(repo => repo.BuscarPorId(checkListPatchDto.Id), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map(checkListPatchDto, checkListModel), Times.Once);
        }

        [Fact]
        public async Task ApagarAsync_DeveApagarCheckList()
        {
            // Arrange
            _repositoryCheckListMock.Setup(repo => repo.Apagar(It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var result = await _checkListService.ApagarAsync(1);

            // Assert
            Assert.True(result);
            _repositoryCheckListMock.Verify(repo => repo.Apagar(1), Times.Once);
        }

        [Fact]
        public async Task ApagarTodosAsync_DeveApagarTodosCheckLists()
        {
            // Arrange
            _repositoryCheckListMock.Setup(repo => repo.ApagarTodos())
                .ReturnsAsync(true);

            // Act
            var result = await _checkListService.ApagarTodosAsync();

            // Assert
            Assert.True(result);
            _repositoryCheckListMock.Verify(repo => repo.ApagarTodos(), Times.Once);
        }
    }
}
