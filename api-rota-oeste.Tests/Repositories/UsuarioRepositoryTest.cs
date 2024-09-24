using api_rota_oeste.Data;
using api_rota_oeste.Models.Usuario;
using api_rota_oeste.Repositories;
using AutoMapper;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace api_rota_oeste.Tests.Repositories
{
    public class UsuarioRepositoryTest
    {
        private readonly ApiDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioRepositoryTest()
        {
            // Configuração do DbContext para usar o InMemoryDatabase
            var options = new DbContextOptionsBuilder<ApiDBContext>()
                .UseInMemoryDatabase(databaseName: "ApiRotaOesteTestDB")
                .Options;

            _dbContext = new ApiDBContext(options);

            // Configurar AutoMapper (opcional: pode ser um mock ou a configuração real)
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UsuarioModel, UsuarioResponseDTO>();
                cfg.CreateMap<UsuarioRequestDTO, UsuarioModel>();
            });

            _mapper = mapperConfig.CreateMapper();

            // Inicializando o repositório
            _usuarioRepository = new UsuarioRepository(_dbContext, _mapper);
        }

        [Fact]
        public async Task Adicionar_DeveRetornarUsuarioResponseDTO()
        {
            // Arrange: Criar o request DTO com dados fictícios
            var usuarioRequest = new UsuarioRequestDTO(
                "66992337652",
                "teste user",
                "122123",
                null
            );

            // Act: Adicionar o usuário ao banco de dados em memória
            var result = await _usuarioRepository.Adicionar(usuarioRequest);

            // Assert: Verificar se o retorno é o esperado
            Assert.NotNull(result);
            Assert.Equal("teste user", result.Nome);
            Assert.Equal("66992337652", result.Telefone);

            // Verifica se o usuário foi persistido corretamente no banco de dados
            var usuarioNoBanco = await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.Telefone == "66992337652");
            Assert.NotNull(usuarioNoBanco);
            Assert.Equal("teste user", usuarioNoBanco.Nome);
        }
    }
}
