using System.Collections.Generic;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services;
using api_rota_oeste.Services.Interfaces;
using AutoMapper;
using Moq;
using Xunit;
using System.Threading.Tasks;
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Data;
using api_rota_oeste.Repositories;
using AutoMapper;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using api_rota_oeste.Models.Questao;

namespace api_rota_oeste.Tests.Services;

public class QuestaoServiceTest
{
    private readonly ApiDBContext _dbContext;
    private readonly Mock<IQuestaoRepository> _questaoRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly QuestaoService _questaoService;
    private readonly QuestaoRepository _questaoRepository;

    public QuestaoServiceTest()
    {
        // Configuração do DbContext para usar o InMemoryDatabase
        var options = new DbContextOptionsBuilder<ApiDBContext>()
            .UseInMemoryDatabase(databaseName: "ApiRotaOesteTestDB")
            .Options;

        _dbContext = new ApiDBContext(options);
        
        _questaoRepositoryMock = new Mock<IQuestaoRepository>();
        _mapperMock = new Mock<IMapper>();
        _questaoService = new QuestaoService(_questaoRepositoryMock.Object, _mapperMock.Object);
        _questaoRepository = new QuestaoRepository(_dbContext);
    }

    [Fact]
    public async Task criar_questao()
    {
        var questaoDTO = new QuestaoRequestDTO("tituloteste", "tipoteste");
        Assert.NotNull(questaoDTO);
        var questao = new QuestaoModel("tituloteste", "tipoteste");
        
        _mapperMock.Setup(mapper => mapper.Map<QuestaoModel>(questaoDTO))
            .Returns(questao);
        
        _questaoRepository.criar(questao);

        var questaoSalva = _questaoRepository.obter(1);
        Assert.Equal("tituloteste", questaoSalva.Titulo);

    }

    [Fact]
    public async Task listar_questoes()
    {
        var questao1 = new QuestaoModel("tituloteste", "tipoteste");
        var questao2 = new QuestaoModel("tituloteste2", "tipoteste2");
        var listaQuestoes = new List<QuestaoModel>() { questao1, questao2 };
        _questaoRepository.criar(questao1);
        _questaoRepository.criar(questao2);
        
        _questaoRepositoryMock.Setup(repo => repo.listar())
            .Returns(listaQuestoes);
        
        var questoes = _questaoService.listar();
        Assert.NotNull(questoes);

        Assert.Equal("tituloteste", questoes[0].Titulo);
        Assert.Equal("tituloteste2", questoes[1].Titulo);
    }
    
    [Fact]
    public async Task obter_questao_por_id()
    {
        var questao1 = new QuestaoModel("tituloteste", "tipoteste");
        _questaoRepository.criar(questao1);
        
        var questao2 = new QuestaoResponseDTO("tituloteste", "tipoteste");
        
        
        _questaoRepositoryMock.Setup(repo => repo.obter(1))
            .Returns(questao1);
        
        _mapperMock.Setup(mapper => mapper.Map<QuestaoResponseDTO>(questao1))
            .Returns(questao2);
        
        var questao = _questaoService.obter(1);

        Assert.Equal(questao, questao2);
    }
    
    [Fact]
    public async Task deletar_questao_por_id()
    {
        var questao1 = new QuestaoModel("tituloteste", "tipoteste");
        _questaoRepository.criar(questao1);
        
        _questaoRepositoryMock.Setup(repo => repo.obter(1))
            .Returns(questao1);
        
        // Mockar o método deletar para confirmar que o método será chamado
        _questaoRepositoryMock.Setup(repo => repo.deletar(questao1.Id))
            .Verifiable();  // Verificará se o método foi chamado durante o teste
        
        // Act
        _questaoService.deletar(questao1.Id);
        
        // Assert
        // Verifica se o método obter foi chamado com o ID 1
        _questaoRepositoryMock.Verify(repo => repo.obter(1), Times.Once);
    
        // Verifica se o método deletar foi chamado com o ID da questão
        _questaoRepositoryMock.Verify(repo => repo.deletar(questao1.Id), Times.Once);
    }
    
    [Fact]
    public void deletar_questao_por_id_nao_existe()
    {
        // Arrange
        _questaoRepositoryMock.Setup(repo => repo.obter(It.IsAny<int>()))
            .Returns((QuestaoModel)null); // Retorna null para simular que a questão não foi encontrada
    
        // Act & Assert
        var exception = Assert.Throws<KeyNotFoundException>(() => _questaoService.deletar(1));

        // Verifica se a mensagem de exceção está correta
        Assert.Equal("Não há questão registrada com o ID informado.", exception.Message);

        // Verifica se o método obter foi chamado com o ID 1
        _questaoRepositoryMock.Verify(repo => repo.obter(1), Times.Once);

        // Verifica que o método deletar não foi chamado, pois a questão não foi encontrada
        _questaoRepositoryMock.Verify(repo => repo.deletar(It.IsAny<int>()), Times.Never);
    }

    
    [Fact]
    public async Task editar_questao_por_id_e_dto()
    {
        var questao1 = new QuestaoRequestDTO("tituloteste", "tipoteste");
        var questao2 = new QuestaoModel("tituloteste", "tipoteste");
        _questaoRepository.criar(questao2);
        
        // Mockar o método deletar para confirmar que o método será chamado
        _questaoRepositoryMock.Setup(repo => repo.salvar())
            .Verifiable();  // Verificará se o método foi chamado durante o teste
        
        _questaoRepositoryMock.Setup(repo => repo.obter(1))
            .Returns(questao2);
        
        _questaoService.editar(1, questao1);
        
        Assert.Equal(_questaoRepository.obter(1), questao2);
        _questaoRepositoryMock.Verify(repo => repo.obter(1), Times.Once);
        _questaoRepositoryMock.Verify(repo => repo.salvar(), Times.Once);
    }
    
}