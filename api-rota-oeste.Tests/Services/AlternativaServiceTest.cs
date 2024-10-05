using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_rota_oeste.Models.Alternativa;
using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Models.RespostaAlternativa;
using api_rota_oeste.Repositories;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services;
using api_rota_oeste.Services.Interfaces;
using AutoMapper;
using Moq;
using Xunit;

public class AlternativaServiceTest
{
    private readonly Mock<IAlternativaRepository> _repositoryAlternativaMock;
    private readonly Mock<IQuestaoRepository> _repositoryQuestaoMock;
    private readonly Mock<IRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AlternativaService _alternativaService;

    public AlternativaServiceTest()
    {
        _repositoryAlternativaMock = new Mock<IAlternativaRepository>();
        _repositoryQuestaoMock = new Mock<IQuestaoRepository>();
        _repositoryMock = new Mock<IRepository>();
        _mapperMock = new Mock<IMapper>();

        _alternativaService = new AlternativaService(
            _repositoryAlternativaMock.Object,
            _mapperMock.Object,
            _repositoryQuestaoMock.Object,
            _repositoryMock.Object
        );
    }

    [Fact]
    public async Task AdicionarAsync_DeveAdicionarAlternativa()
    {
        // Arrange
        var alternativaRequest = new AlternativaRequestDTO(1, "Descrição de teste");
        var questaoModel = new QuestaoModel { Id = 1, Titulo = "Questão Teste", Tipo = TipoQuestao.QUESTAO_OBJETIVA };
        var alternativaModel = new AlternativaModel(alternativaRequest, questaoModel, 1);
        var alternativaResponse = new AlternativaResponseDTO(1, 1, "Descrição de teste", 1, null, null);

        _repositoryQuestaoMock.Setup(repo => repo.BuscarPorId(alternativaRequest.QuestaoId))
            .ReturnsAsync(questaoModel);
        _repositoryAlternativaMock.Setup(repo => repo.Adicionar(It.IsAny<AlternativaModel>()))
            .ReturnsAsync(alternativaModel);
        _repositoryAlternativaMock.Setup(repo => repo.ObterProximoCodigoPorQuestaoId(alternativaRequest.QuestaoId))
            .ReturnsAsync(1);
        _mapperMock.Setup(mapper => mapper.Map<AlternativaResponseDTO>(alternativaModel))
            .Returns(alternativaResponse);

        // Act
        var result = await _alternativaService.AdicionarAsync(alternativaRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(alternativaRequest.Descricao, result.Descricao);
        _repositoryQuestaoMock.Verify(repo => repo.BuscarPorId(alternativaRequest.QuestaoId), Times.Once);
        _repositoryAlternativaMock.Verify(repo => repo.Adicionar(It.IsAny<AlternativaModel>()), Times.Once);
    }

    [Fact]
    public async Task BuscarPorIdAsync_DeveRetornarAlternativa()
    {
        // Arrange
        var alternativaModel = new AlternativaModel
        {
            Id = 1,
            QuestaoId = 1,
            Descricao = "Descrição de teste",
            Codigo = 1,
            Questao = new QuestaoModel { Id = 1, Titulo = "Questão Teste", Tipo = TipoQuestao.QUESTAO_MULTIPLA_ESCOLHA }
        };
        var alternativaResponse = new AlternativaResponseDTO(1, 1, "Descrição de teste", 1, null, null);

        _repositoryAlternativaMock.Setup(repo => repo.BuscarPorId(1))
            .ReturnsAsync(alternativaModel);
        _mapperMock.Setup(mapper => mapper.Map<AlternativaResponseDTO>(alternativaModel))
            .Returns(alternativaResponse);

        // Act
        var result = await _alternativaService.BuscarPorIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(alternativaModel.Id, result.Id);
        Assert.Equal(alternativaModel.Descricao, result.Descricao);
        _repositoryAlternativaMock.Verify(repo => repo.BuscarPorId(1), Times.Once);
    }

    [Fact]
    public async Task BuscarTodosAsync_DeveRetornarListaDeAlternativas()
    {
        // Arrange
        var alternativas = new List<AlternativaModel>
        {
            new AlternativaModel { Id = 1, QuestaoId = 1, Descricao = "Alternativa 1", Codigo = 1 },
            new AlternativaModel { Id = 2, QuestaoId = 1, Descricao = "Alternativa 2", Codigo = 2 }
        };
        var alternativasResponse = alternativas.Select(a => new AlternativaResponseDTO(a.Id, a.QuestaoId, a.Descricao, a.Codigo, null, null)).ToList();

        _repositoryAlternativaMock.Setup(repo => repo.BuscarTodos())
            .ReturnsAsync(alternativas);
        _mapperMock.Setup(mapper => mapper.Map<AlternativaResponseDTO>(It.IsAny<AlternativaModel>()))
            .Returns((AlternativaModel a) => alternativasResponse.First(r => r.Id == a.Id));

        // Act
        var result = await _alternativaService.BuscarTodosAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        _repositoryAlternativaMock.Verify(repo => repo.BuscarTodos(), Times.Once);
    }


    [Fact]
    public async Task AtualizarAsync_DeveAtualizarAlternativa()
    {
        // Arrange
        var alternativaPatch = new AlternativaPatchDTO(1, "Nova descrição");
        var alternativaModel = new AlternativaModel { Id = 1, QuestaoId = 1, Descricao = "Descrição antiga", Codigo = 1 };

        _repositoryAlternativaMock.Setup(repo => repo.BuscarPorId(alternativaPatch.Id))
            .ReturnsAsync(alternativaModel);
        _mapperMock.Setup(mapper => mapper.Map(alternativaPatch, alternativaModel))
            .Callback<AlternativaPatchDTO, AlternativaModel>((patch, model) =>
            {
                model.Descricao = patch.Descricao ?? model.Descricao;
            });

        // Act
        var result = await _alternativaService.AtualizarAsync(alternativaPatch);

        // Assert
        Assert.True(result);
        Assert.Equal("Nova descrição", alternativaModel.Descricao);
        _repositoryAlternativaMock.Verify(repo => repo.BuscarPorId(alternativaPatch.Id), Times.Once);
        _repositoryMock.Verify(repo => repo.Salvar(), Times.Once);
    }


    [Fact]
    public async Task ApagarAsync_DeveApagarAlternativa()
    {
        // Arrange
        var alternativaId = 1;

        _repositoryQuestaoMock.Setup(repo => repo.BuscarPorId(alternativaId))
            .ReturnsAsync(new QuestaoModel { Id = alternativaId });

        // Act
        var result = await _alternativaService.ApagarAsync(alternativaId);

        // Assert
        Assert.True(result);
        _repositoryQuestaoMock.Verify(repo => repo.BuscarPorId(alternativaId), Times.Once);
        _repositoryQuestaoMock.Verify(repo => repo.Apagar(alternativaId), Times.Once);
    }
}
