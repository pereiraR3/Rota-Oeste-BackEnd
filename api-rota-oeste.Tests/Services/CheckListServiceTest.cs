using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services;
using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace api_rota_oeste.Tests.Services
{
    public class CheckListServiceTest
    {
        private readonly Mock<ICheckListRepository> _repMock;
        private readonly Mock<IMapper> _mapMock;
        private readonly CheckListService _service;

        public CheckListServiceTest()
        {
            _repMock = new Mock<ICheckListRepository>();
            _mapMock = new Mock<IMapper>();
            _service = new CheckListService(_repMock.Object, _mapMock.Object);
        }

        [Fact]
        public async Task AddAsync_DeveAdicionarCheck()
        {
            // Arrange
            var checkReq= new CheckListRequestDTO("Nome", DateTime.Now, 1);
            var checkModel = new CheckListModel { Id = 1, Nome = "Teste", DataCriacao = DateTime.Now};
            var checkResponse = new CheckListResponseDTO(1, 1, "Teste", DateTime.Now);

            _repMock.Setup(repo => repo.Add(checkReq))
                .ReturnsAsync(checkModel);

            _mapMock.Setup(mapper => mapper.Map < CheckListResponseDTO >(checkModel))
                .Returns(checkResponse);

            var result = await _service.AddAsync(checkReq);

            Assert.NotNull(result);
            Assert.Equal(checkResponse.Id, result.Id);
            _repMock.Verify(repo => repo.Add(checkReq), Times.Once);
        }

        [Fact]
        public async Task AddCollectionAsync_DeveAdicionarChecks()
        {
            // Arrange
            var checkReq = new CheckListCollectionDTO(new List<CheckListRequestDTO>
            {
                new CheckListRequestDTO("Nome", DateTime.Now, 1),
                new CheckListRequestDTO("Nome2", DateTime.Now, 2)
            });

            var checksModel = new List<CheckListModel>
            {
                new CheckListModel { Id = 1, Nome = "Teste", DataCriacao = DateTime.Now},
                new CheckListModel { Id = 2, Nome = "Teste2", DataCriacao = DateTime.Now}
            };

            _repMock.Setup(repo => repo.AddCollection(checkReq))
                .ReturnsAsync(checksModel);

            var checksResponse = checksModel.Select(c => new CheckListResponseDTO(c.Id, c.UsuarioId, c.Nome, c.DataCriacao)).ToList();

            _mapMock.Setup(mapper => mapper.Map<CheckListResponseDTO>(It.IsAny<CheckListModel>()))
                .Returns((CheckListModel source) => new CheckListResponseDTO(source.Id, source.UsuarioId, source.Nome, source.DataCriacao));

            // Act
            var result = await _service.AddCollectionAsync(checkReq);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            _repMock.Verify(repo => repo.AddCollection(checkReq), Times.Once);
        }


        [Fact]
        public async Task FindByIdAsync_DeveRetornarCheck()
        {
            // Arrange
            var checkModel = new CheckListModel { Id = 1, Nome = "Teste", DataCriacao = DateTime.Now };
            var checkResponse = new CheckListResponseDTO(1, 1, "Teste", DateTime.Now);

            _repMock.Setup(repo => repo.FindById(1))
                .ReturnsAsync(checkModel);

            _mapMock.Setup(mapper => mapper.Map<CheckListResponseDTO>(checkModel))
                .Returns(checkResponse);

            // Act
            var result = await _service.FindByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(checkResponse.Id, result.Id);
            _repMock.Verify(repo => repo.FindById(1), Times.Once);
        }

        [Fact]
        public async Task FindByIdAsync_DeveLancarExcecao_SeNaoEncontrado()
        {
            // Arrange
            _repMock.Setup(repo => repo.FindById(It.IsAny<int>()))
                .ReturnsAsync((CheckListModel)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.FindByIdAsync(1));
        }

        [Fact]
        public async Task DeleteAsync_DeveApagarCheck()
        {
            // Arrange
            _repMock.Setup(repo => repo.Delete(It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteAsync(1);

            // Assert
            Assert.True(result);
            _repMock.Verify(repo => repo.Delete(1), Times.Once);
        }

        [Fact]
        public async Task ApagarAsync_DeveLancarExcecao_SeFalhar()
        {
            // Arrange
            _repMock.Setup(repo => repo.Delete(It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<ApplicationException>(() => _service.DeleteAsync(1));
        }

        [Fact]
        public async Task DeleteAllAsync_DeveApagarTodosChecks()
        {
            // Arrange
            _repMock.Setup(repo => repo.DeleteAll())
                .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteAllAsync();

            // Assert
            Assert.True(result);
            _repMock.Verify(repo => repo.DeleteAll(), Times.Once);
        }

        [Fact]
        public async Task DeleteAllAsync_DeveLancarExcecao_SeFalhar()
        {
            // Arrange
            _repMock.Setup(repo => repo.DeleteAll())
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<ApplicationException>(() => _service.DeleteAllAsync());
        }

        [Fact]
        public async Task GetAllAsync_DeveRetornarTodosChecks()
        {
            // Arrange
            var checksModel = new List<CheckListModel>
            {
                new CheckListModel { Id = 1 },
                new CheckListModel { Id = 2 }
            };

            _repMock.Setup(repo => repo.GetAll())
                .ReturnsAsync(checksModel);

            var checksResponse = checksModel.Select(c => new CheckListResponseDTO(c.Id, c.UsuarioId, c.Nome, c.DataCriacao)).ToList();

            _mapMock.Setup(mapper => mapper.Map<CheckListResponseDTO>(It.IsAny<CheckListModel>()))
                .Returns((CheckListModel source) => new CheckListResponseDTO(source.Id, source.UsuarioId, source.Nome, source.DataCriacao));

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetAllAsync_DeveLancarExcecao_SeNaoEncontrarChecks()
        {
            // Arrange
            _repMock.Setup(repo => repo.GetAll())
                .ReturnsAsync(new List<CheckListModel>());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.GetAllAsync());
        }

    }
}
