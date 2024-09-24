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
                cfg.CreateMap<UsuarioPatchDTO, UsuarioModel>()
                    .ForAllMembers(ops => ops.Condition((src, dest, srcMember) => srcMember != null));
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

        [Fact]
        public async Task BuscarPorId_DeveRetornarUsuarioModel()
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
            var usuarioModel = await _usuarioRepository.BuscaPorId(result.Id);
            
            //Assert: verifica se o retorno é o esperado
            Assert.NotNull(usuarioModel);
            Assert.Equal(usuarioRequest.Nome, usuarioModel.Nome);
            Assert.Equal(usuarioRequest.Telefone, usuarioModel.Telefone);

        }

        [Fact]
        public async Task BuscarPorId_DeveRetornarNotFound()
        {
            
            // Arrange: Criar o request DTO com dados fictícios
            
            /*
             * Neste caso não haverá dados, pois vamos forçar um Not Found
             */
            
            // Act: Adicionar o usuário ao banco de dados em memória
            var usuarioModel = await _usuarioRepository.BuscaPorId(1);
            
            // Assert: verifica se o retorno é o esperado
            Assert.Null(usuarioModel);
            
        }

        [Fact]
        public async Task Atualizar_DeveRetornarTrue()
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
    
            // Atualizar parcialmente o usuário com um patch
            var usuarioPatch = new UsuarioPatchDTO
            {
                Id = result.Id,
                Telefone = "66992337652",  // Mantemos o telefone o mesmo
                Nome = null,               // Nome não será atualizado
                Foto = null                // Foto também não será atualizada
            };
    
            // Executa a atualização parcial
            var resultPatch = await _usuarioRepository.Atualizar(usuarioPatch);
    
            // Recupera o usuário atualizado do banco de dados
            var usuarioBanco = await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.Id == result.Id);
    
            // Assert: Verifica se o retorno é o esperado
            Assert.NotNull(result);
            Assert.True(resultPatch);
            Assert.Equal(usuarioPatch.Telefone, usuarioBanco.Telefone);
            Assert.Equal(usuarioRequest.Nome, usuarioBanco.Nome);
        }
        
        [Fact]
        public async Task Atualizar_DeveRetornarFalse()
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
    
            // Atualizar parcialmente o usuário com um patch
            var usuarioPatch = new UsuarioPatchDTO
            {
                Id = 100,
                Telefone = "66992337652",  // Mantemos o telefone o mesmo
                Nome = null,               // Nome não será atualizado
                Foto = null                // Foto também não será atualizada
            };
    
            // Executa a atualização parcial
            var resultPatch = await _usuarioRepository.Atualizar(usuarioPatch);
            
            // Assert: Verifica se o retorno é o esperado      
            Assert.False(resultPatch);
            
        }
        
        [Fact]
        public async Task Apagar_DeveRetornarTrue()
        {

            // Arrange: Criar o request DTO com dados fictícios
            var usuarioRequest = new UsuarioRequestDTO(
                "66992337652",
                "teste user",
                "122123",
                null
            );

            // Act: Adicionar o usuário ao banco de dados em memória e removê-lo
            var result = await _usuarioRepository.Adicionar(usuarioRequest);
            var statusApagar = await _usuarioRepository.Apagar(result.Id);
            var usuarioBanco = await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.Telefone == "66992337652");

            // Assert: verifica se o retorno é o esperado
            Assert.True(statusApagar);
            Assert.Null(usuarioBanco);

        }

        [Fact]
        public async Task Apagar_DeveRetornarFalse()
        {
            
            // Arrange: Criar o request DTO com dados fictícios
            var usuarioRequest = new UsuarioRequestDTO(
                "66992337652",
                "teste user",
                "122123",
                null
            );

            // Act: Adicionar o usuário ao banco de dados em memória e removê-lo
            var result = await _usuarioRepository.Adicionar(usuarioRequest);
            var statusApagar = await _usuarioRepository.Apagar(3);
            var usuarioBanco = await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.Id == 1);
            
            // Assert: verifica se o retorno é o esperado
            Assert.NotNull(result);
            Assert.False(statusApagar);
            Assert.NotNull(usuarioBanco);
            
        }
        

    }
}
