using System;
using System.Threading.Tasks;
using api_rota_oeste.Data;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Models.Usuario;
using api_rota_oeste.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace api_rota_oeste.Tests.Repositories;

public class InteracaoRepositoryTest
{
    private readonly ApiDBContext _dbContext;
    private readonly IMapper _mapper;
    private readonly InteracaoRepository _interacaoRepository;
    private readonly UsuarioRepository _usuarioRepository;
    private readonly ClienteRepository _clienteRepository;

    public InteracaoRepositoryTest()
    {
        // Configuração do DbContext para usar o InMemoryDatabase
        var options = new DbContextOptionsBuilder<ApiDBContext>()
            .UseInMemoryDatabase(databaseName: "ApiRotaOesteTestDB")
            .Options;

        _dbContext = new ApiDBContext(options);

        // Configurar AutoMapper (opcional: pode ser um mock ou a configuração real)
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<InteracaoRequestDTO, InteracaoModel>()
                .ForAllMembers(ops => ops.Condition((src, dest, srcMember) => srcMember != null));
        });

        // Inicializando o repositório
        _interacaoRepository = new InteracaoRepository(_dbContext);
        _usuarioRepository = new UsuarioRepository(_dbContext, _mapper);
        _clienteRepository = new ClienteRepository(_mapper, _dbContext, _usuarioRepository);
    }
    
    [Fact]
    public async Task Adicionar()
    {
        
        var usuario = new UsuarioRequestDTO("teste", "teste", "123", null);

        _usuarioRepository.Adicionar(usuario);
        
        var request = new ClienteRequestDTO(1, "teste", "teste", null);
        
        _clienteRepository.Adicionar(request);

        var cliente = await _clienteRepository.BuscaPorId(1);
        
        // Arrange: Criar  com dados fictícios
        var interacao = new InteracaoModel();
        interacao.Status = true;
        var data = DateTime.Now;
        interacao.Data = data;
        interacao.cliente = cliente;
        
        
        // Act: Adicionar a interacao ao banco de dados em memória
        _interacaoRepository.criar(interacao);

        // Verifica se a interacao foi persistido corretamente no banco de dados
        var interacaoNoBanco = await _dbContext.Interacoes.FirstOrDefaultAsync(u => u.cliente == cliente && u.Status == true);
        Assert.NotNull(interacaoNoBanco);
        Assert.Equal(data, interacaoNoBanco.Data);
    }
}