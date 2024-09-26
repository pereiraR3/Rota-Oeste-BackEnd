using api_rota_oeste.Data;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Repositories;
using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.Usuario;
using Xunit;
using api_rota_oeste.Models.CheckList;

namespace api_rota_oeste.Tests.Repositories
{
    public class CheckListRepositoryTest
    {
        private readonly ApiDBContext _context;
        private readonly Mock<IMapper> _mapMock;
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly CheckListRepository _checkRepository;

        public CheckListRepositoryTest()
        {
            // Configuração do DbContext para usar o InMemoryDatabase
            var options = new DbContextOptionsBuilder<ApiDBContext>()
                .UseInMemoryDatabase(databaseName: "ApiRotaOesteTestDB")
            .Options;

            _context = new ApiDBContext(options);

            // Limpa os dados antes de cada teste
            _context.CheckLists.RemoveRange(_context.CheckLists);
            _context.SaveChanges();

            // Mock para IMapper e IUsuarioRepository
            _mapMock = new Mock<IMapper>();
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();

            // Instancia o repositório a ser testado
            _checkRepository = new CheckListRepository(
                _mapMock.Object,
                _context,
                _usuarioRepositoryMock.Object);
        }

        [Fact]
        public async Task Add_DeveAdicionarCheck()
        {
            // Arrange
            var checkRequest = new CheckListRequestDTO("Nome", DateTime.Now, 1);
            var usuario = new UsuarioModel { Id = 2, Nome = "Usuario Teste1", Senha = "122123", Telefone = "123456789" };

            _usuarioRepositoryMock.Setup(repo => repo.BuscaPorId(It.IsAny<int>()))
                .ReturnsAsync(usuario);

            var checkModel = new CheckListModel(checkRequest, usuario);

            // Act
            var result = await _checkRepository.Add(checkRequest);

            // Assert
            _usuarioRepositoryMock.Verify(repo => repo.BuscaPorId(It.IsAny<int>()), Times.Once);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task AddCollection_DeveAdicionarChecks()
        {
            // Arrange
            var checkRequest1 = new CheckListRequestDTO("Nome", DateTime.Now, 1);
            var checkRequest2 = new CheckListRequestDTO("Outro Nome", DateTime.Now, 2);
            var checkCollection = new CheckListCollectionDTO(new List<CheckListRequestDTO> { checkRequest1, checkRequest2 });

            var usuario = new UsuarioModel { Id = 1, Nome = "Usuário Teste", Senha = "122123", Telefone = "123456789" };

            _usuarioRepositoryMock.Setup(repo => repo.BuscaPorId(It.IsAny<int>()))
                .ReturnsAsync(usuario);

            // Act
            var result = await _checkRepository.AddCollection(checkCollection);

            // Assert
            _usuarioRepositoryMock.Verify(repo => repo.BuscaPorId(It.IsAny<int>()), Times.Once);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task FindById_DeveRetornarCheck()
        {
            // Arrange
            var check = new CheckListModel{ Id = 1, Nome = "Teste", DataCriacao = DateTime.Now};
            _context.CheckLists.Add(check);
            await _context.SaveChangesAsync();

            // Act
            var result = await _checkRepository.FindById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(check.Id, result.Id);
        }

        [Fact]
        public async Task FindById_DeveRetornarNullSeNaoEncontrado()
        {
            // Act
            var result = await _checkRepository.FindById(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAll_DeveRetornarTodosChecks()
        {
            // Arrange
            var checks = new List<CheckListModel>
            {
                new CheckListModel { Id = 1, Nome = "Teste", DataCriacao = DateTime.Now},
                new CheckListModel { Id = 2, Nome = "Teste", DataCriacao = DateTime.Now}
            };
            _context.CheckLists.AddRange(checks);
            await _context.SaveChangesAsync();

            // Act
            var result = await _checkRepository.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task Delete_DeveRemoverCheck()
        {
            // Arrange
            var check = new CheckListModel { Id = 1, Nome = "Teste", DataCriacao = DateTime.Now };
            _context.CheckLists.Add(check);
            await _context.SaveChangesAsync();

            // Act
            var result = await _checkRepository.Delete(1);

            // Assert
            Assert.True(result);
            Assert.DoesNotContain(check, _context.CheckLists);
        }

        [Fact]
        public async Task Delete_DeveRetornarFalseSeNaoEncontrado()
        {
            // Act
            var result = await _checkRepository.Delete(1);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAll_DeveRemoverTodosChecks()
        {
            // Arrange
            var checks = new List<CheckListModel>
            {
                new CheckListModel { Id = 1, Nome = "Teste", DataCriacao = DateTime.Now},
                new CheckListModel { Id = 2, Nome = "Teste", DataCriacao = DateTime.Now}
            };
            _context.CheckLists.AddRange(checks);
            await _context.SaveChangesAsync();

            // Act
            var result = await _checkRepository.DeleteAll();

            // Assert
            Assert.True(result);
            Assert.Empty(_context.CheckLists);
        }

    }
}
