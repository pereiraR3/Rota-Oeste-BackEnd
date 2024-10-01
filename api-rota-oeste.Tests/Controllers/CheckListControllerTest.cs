using System;
using api_rota_oeste.Controllers;
using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api_rota_oeste.Tests.Controllers
{
    public class CheckListControllerTest
    {
        private readonly Mock<ICheckListService> _checkListServiceMock;
        private readonly CheckListController _controller;

        public CheckListControllerTest()
        {
            _checkListServiceMock = new Mock<ICheckListService>();
            _controller = new CheckListController(_checkListServiceMock.Object);
        }

        [Fact]
        public async Task Adicionar_DeveRetornarCreated()
        {
            // Arrange
            var checkListRequest = new CheckListRequestDTO(1, "Checklist Teste");
            var checkListResponse = new CheckListResponseDTO(1, 1, "Checklist Teste", DateTime.Now, null, null);

            _checkListServiceMock.Setup(service => service.AdicionarAsync(checkListRequest))
                .ReturnsAsync(checkListResponse);

            // Act
            var result = await _controller.Adicionar(checkListRequest);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(201, createdAtActionResult.StatusCode);
            Assert.Equal(checkListResponse, createdAtActionResult.Value);
        }

        [Fact]
        public async Task BuscarPorId_DeveRetornarOkSeEncontrado()
        {
            // Arrange
            var checkListResponse = new CheckListResponseDTO(1, 1, "Checklist Teste", DateTime.Now, null, null);

            _checkListServiceMock.Setup(service => service.BuscarPorIdAsync(1))
                .ReturnsAsync(checkListResponse);

            // Act
            var result = await _controller.BuscarPorId(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(checkListResponse, okResult.Value);
        }

        [Fact]
        public async Task BuscarPorId_DeveRetornarNotFoundSeNaoEncontrado()
        {
            // Arrange
            _checkListServiceMock.Setup(service => service.BuscarPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync((CheckListResponseDTO)null);

            // Act
            var result = await _controller.BuscarPorId(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task BuscarTodos_DeveRetornarOkComChecklists()
        {
            // Arrange
            var checkListsResponse = new List<CheckListResponseDTO>
            {
                new CheckListResponseDTO(1, 1, "Checklist 1", DateTime.Now, null, null),
                new CheckListResponseDTO(2, 1, "Checklist 2", DateTime.Now, null, null)
            };

            _checkListServiceMock.Setup(service => service.BuscarTodosAsync())
                .ReturnsAsync(checkListsResponse);

            // Act
            var result = await _controller.BuscarTodos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(checkListsResponse, okResult.Value);
        }

        [Fact]
        public async Task Delete_DeveRetornarNoContentSeApagarComSucesso()
        {
            // Arrange
            _checkListServiceMock.Setup(service => service.ApagarAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_DeveRetornarNotFoundSeNaoEncontrado()
        {
            // Arrange
            _checkListServiceMock.Setup(service => service.ApagarAsync(It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Atualizar_DeveRetornarNoContentSeAtualizarComSucesso()
        {
            // Arrange
            var checkListPatchDto = new CheckListPatchDTO(1, "Novo Nome");
            var checkListResponse = new CheckListResponseDTO(1, 1, "Novo Nome", DateTime.Now, null, null);

            _checkListServiceMock.Setup(service => service.BuscarPorIdAsync(checkListPatchDto.Id))
                .ReturnsAsync(checkListResponse);

            _checkListServiceMock.Setup(service => service.AtualizarAsync(checkListPatchDto))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Atualizar(checkListPatchDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Atualizar_DeveRetornarNotFoundSeNaoEncontrado()
        {
            // Arrange
            var checkListPatchDto = new CheckListPatchDTO(1, "Novo Nome");

            _checkListServiceMock.Setup(service => service.BuscarPorIdAsync(checkListPatchDto.Id))
                .ReturnsAsync((CheckListResponseDTO)null);

            // Act
            var result = await _controller.Atualizar(checkListPatchDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Questão não encontrada", notFoundResult.Value);
        }

        [Fact]
        public async Task DeleteAll_DeveRetornarNoContentSeApagarTodosComSucesso()
        {
            // Arrange
            _checkListServiceMock.Setup(service => service.ApagarTodosAsync())
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteAll();

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteAll_DeveRetornarNotFoundSeNaoExistiremChecklists()
        {
            // Arrange
            _checkListServiceMock.Setup(service => service.ApagarTodosAsync())
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteAll();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
