using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services;
using api_rota_oeste.Services.Interfaces;
using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace api_rota_oeste.Tests.Services
{
    public class InteracaoServiceTest
    {
        private readonly Mock<IInteracaoRepository> _interacaoRepositoryMock;
        private readonly Mock<IClienteRepository> _clienteRepositoryMock;
        private readonly Mock<ICheckListRepository> _checkListRepositoryMock;
        private readonly Mock<IRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly InteracaoService _interacaoService;

        public InteracaoServiceTest()
        {
            _interacaoRepositoryMock = new Mock<IInteracaoRepository>();
            _clienteRepositoryMock = new Mock<IClienteRepository>();
            _checkListRepositoryMock = new Mock<ICheckListRepository>();
            _repositoryMock = new Mock<IRepository>();
            _mapperMock = new Mock<IMapper>();
            _interacaoService = new InteracaoService(
                _interacaoRepositoryMock.Object,
                _mapperMock.Object,
                _clienteRepositoryMock.Object,
                _checkListRepositoryMock.Object,
                _repositoryMock.Object
            );
        }

        [Fact]
        public async Task AdicionarAsync_DeveAdicionarInteracao()
        {
            // Arrange
            var interacaoRequest = new InteracaoRequestDTO(1, 1, true);
            var clienteModel = new ClienteModel
            {
                Id = 1, Nome = "Cliente Teste", Telefone = "123456789", UsuarioId = 1
            };
            var checkListModel = new CheckListModel
            {
                Id = 1, Nome = "Checklist Teste", UsuarioId = 1, DataCriacao = DateTime.Now
            };
            var interacaoModel = new InteracaoModel(interacaoRequest, clienteModel, checkListModel);
            var interacaoResponse = new InteracaoResponseDTO(1, 1, 1, true, null, null);

            _clienteRepositoryMock.Setup(repo => repo.BuscarPorId(interacaoRequest.ClienteId))
                .ReturnsAsync(clienteModel);

            _checkListRepositoryMock.Setup(repo => repo.BuscarPorId(interacaoRequest.CheckListId))
                .ReturnsAsync(checkListModel);

            _interacaoRepositoryMock.Setup(repo => repo.Adicionar(It.IsAny<InteracaoModel>()))
                .ReturnsAsync(interacaoModel);

            _mapperMock.Setup(mapper => mapper.Map<InteracaoResponseDTO>(It.IsAny<InteracaoModel>()))
                .Returns(interacaoResponse);

            // Act
            var result = await _interacaoService.AdicionarAsync(interacaoRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(interacaoResponse.Id, result.Id);
            _clienteRepositoryMock.Verify(repo => repo.BuscarPorId(interacaoRequest.ClienteId), Times.Once);
            _checkListRepositoryMock.Verify(repo => repo.BuscarPorId(interacaoRequest.CheckListId), Times.Once);
            _interacaoRepositoryMock.Verify(repo => repo.Adicionar(It.IsAny<InteracaoModel>()), Times.Once);
        }

        [Fact]
        public async Task AdicionarAsync_DeveRetornarNull_SeClienteOuChecklistNaoEncontrado()
        {
            // Arrange
            var interacaoRequest = new InteracaoRequestDTO(1, 1, true);

            _clienteRepositoryMock.Setup(repo => repo.BuscarPorId(It.IsAny<int>()))
                .ReturnsAsync((ClienteModel)null);

            _checkListRepositoryMock.Setup(repo => repo.BuscarPorId(It.IsAny<int>()))
                .ReturnsAsync((CheckListModel)null);

            // Act
            var result = await _interacaoService.AdicionarAsync(interacaoRequest);

            // Assert
            Assert.Null(result);
            _clienteRepositoryMock.Verify(repo => repo.BuscarPorId(interacaoRequest.ClienteId), Times.Once);
            _checkListRepositoryMock.Verify(repo => repo.BuscarPorId(interacaoRequest.CheckListId), Times.Once);
        }

        [Fact]
        public async Task BuscarPorIdAsync_DeveRetornarInteracao_SeEncontrada()
        {
            // Arrange
            var interacaoModel = new InteracaoModel
            {
                Id = 1, ClienteId = 1, CheckListId = 1, Status = true, Data = DateTime.Now
            };
            var interacaoResponse = new InteracaoResponseDTO(1, 1, 1, true, null, null);

            _interacaoRepositoryMock.Setup(repo => repo.BuscarPorId(1))
                .ReturnsAsync(interacaoModel);

            _mapperMock.Setup(mapper => mapper.Map<InteracaoResponseDTO>(interacaoModel))
                .Returns(interacaoResponse);

            // Act
            var result = await _interacaoService.BuscarPorIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(interacaoResponse.Id, result.Id);
            _interacaoRepositoryMock.Verify(repo => repo.BuscarPorId(1), Times.Once);
        }

        [Fact]
        public async Task BuscarPorIdAsync_DeveLancarExcecao_SeNaoEncontrada()
        {
            // Arrange
            _interacaoRepositoryMock.Setup(repo => repo.BuscarPorId(It.IsAny<int>()))
                .ReturnsAsync((InteracaoModel)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _interacaoService.BuscarPorIdAsync(1));
            _interacaoRepositoryMock.Verify(repo => repo.BuscarPorId(1), Times.Once);
        }

        [Fact]
        public async Task AtualizarAsync_DeveRetornarTrue_SeAtualizarComSucesso()
        {
            // Arrange
            var interacaoPatchDto = new InteracaoPatchDTO(1, true);
            var interacaoModel = new InteracaoModel
            {
                Id = 1, ClienteId = 1, CheckListId = 1, Status = false, Data = DateTime.Now
            };

            _interacaoRepositoryMock.Setup(repo => repo.BuscarPorId(interacaoPatchDto.Id))
                .ReturnsAsync(interacaoModel);

            _mapperMock.Setup(mapper => mapper.Map(interacaoPatchDto, interacaoModel))
                .Returns(interacaoModel);

            // Act
            var result = await _interacaoService.AtualizarAsync(interacaoPatchDto);

            // Assert
            Assert.True(result);
            _repositoryMock.Verify(repo => repo.Salvar(), Times.Once);
        }

        [Fact]
        public async Task AtualizarAsync_DeveLancarExcecao_SeFalhar()
        {
            // Arrange
            var interacaoPatchDto = new InteracaoPatchDTO(1, true);

            _interacaoRepositoryMock.Setup(repo => repo.BuscarPorId(It.IsAny<int>()))
                .ThrowsAsync(new KeyNotFoundException("Interação não encontrada"));

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _interacaoService.AtualizarAsync(interacaoPatchDto));
        }
        
    }
}
