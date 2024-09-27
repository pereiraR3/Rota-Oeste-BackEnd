using api_rota_oeste.Controllers;
using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace api_rota_oeste.Tests.Controllers
{
    public class CheckListControllerTest
    {
        private readonly CheckListController _controller;
        private readonly Mock<ICheckListService> _service;

        public CheckListControllerTest()
        {
            _service = new Mock<ICheckListService>();
            _controller = new CheckListController(_service.Object); 
        }

        [Fact]
        public async Task Add_DeveRetornarCreatedAtAction_QuandoCheckListAdicionado()
        {
            // Arrange
            var checkRequest = new CheckListRequestDTO("Teste", DateTime.Now, 1);
            var checkResponse = new CheckListResponseDTO(1, 1, "Teste", DateTime.Now);

            _service
                .Setup(s => s.AddAsync(checkRequest))
                .ReturnsAsync(checkResponse);

            // Act
            var result = await _controller.Add(checkRequest);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("FindById", actionResult.ActionName);
            Assert.Equal(checkResponse, actionResult.Value);
        }

        [Fact]
        public async Task AddCollection_DeveRetornarOk_QuandoChecksAdicionados()
        {
            // Arrange
            var checkCollection = new CheckListCollectionDTO(new List<CheckListRequestDTO>
            {
                new CheckListRequestDTO("Teste1", DateTime.Now, 1),
                new CheckListRequestDTO("Teste2", DateTime.Now, 2)
            });

            var checkResponseList = new List<CheckListResponseDTO>
            {
                new CheckListResponseDTO(1, 1, "Teste1", DateTime.Now),
                new CheckListResponseDTO(2, 2, "Teste2", DateTime.Now)
            };

            _service
                .Setup(s => s.AddCollectionAsync(checkCollection))
                .ReturnsAsync(checkResponseList);

            // Act
            var result = await _controller.AddColletction(checkCollection);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(checkResponseList, actionResult.Value);
        }

        [Fact]
        public async Task FindById_DeveRetornarOk_QuandoCheckEncontrado()
        {
            // Arrange
            var checkResponse = new CheckListResponseDTO(1, 1, "Teste", DateTime.Now);

            _service
                .Setup(s => s.FindByIdAsync(1))
                .ReturnsAsync(checkResponse);

            // Act
            var result = await _controller.FindById(1);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(checkResponse, actionResult.Value);
        }

        [Fact]
        public async Task FindById_DeveRetornarNotFound_QuandoCheckNaoEncontrado()
        {
            // Arrange
            _service
                .Setup(s => s.FindByIdAsync(1))
                .ReturnsAsync((CheckListResponseDTO)null);

            // Act
            var result = await _controller.FindById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Delete_DeveRetornarNoContent_QuandoCheckRemovidoComSucesso()
        {
            // Arrange
            _service
                .Setup(s => s.DeleteAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_DeveRetornarNotFound_QuandoCheckNaoEncontrado()
        {
            // Arrange
            _service
                .Setup(s => s.DeleteAsync(1))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetAll_DeveRetornarOk_QuandoChecksExistem()
        {
            // Arrange
            var checks = new List<CheckListResponseDTO>
            {
                new CheckListResponseDTO(1, 1, "Teste1", DateTime.Now),
                new CheckListResponseDTO(2, 2, "Teste2", DateTime.Now)
            };

            _service
                .Setup(s => s.GetAllAsync())
                .ReturnsAsync(checks);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(checks, actionResult.Value);
        }

        [Fact]
        public async Task GetAll_DeveRetornarNoContent_QuandoNenhumCheckEncontrado()
        {
            // Arrange
            _service
                .Setup(s => s.GetAllAsync())
                .ReturnsAsync((List<CheckListResponseDTO>)null);

            // Act
            var result = await _controller.GetAll();

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

    }
}
