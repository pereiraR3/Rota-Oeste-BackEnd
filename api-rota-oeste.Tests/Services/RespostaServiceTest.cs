using System.Collections.Generic;
using System.Threading.Tasks;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Models.RespostaAlternativa;
using api_rota_oeste.Repositories;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services;
using api_rota_oeste.Services.Interfaces;
using AutoMapper;
using Moq;
using Xunit;

namespace api_rota_oeste.Tests.Services;

public class RespostaServiceTests
{
    private readonly Mock<IRespostaRepository> _mockRespostaAlternativaRepository;
    private readonly Mock<IInteracaoRepository> _mockInteracaoRepository;
    private readonly Mock<IQuestaoRepository> _mockQuestaoRepository;
    private readonly Mock<IRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly RespostaService _service;

    public RespostaServiceTests()
    {
        _mockRespostaAlternativaRepository = new Mock<IRespostaRepository>();
        _mockInteracaoRepository = new Mock<IInteracaoRepository>();
        _mockQuestaoRepository = new Mock<IQuestaoRepository>();
        _mockRepository = new Mock<IRepository>();
        _mockMapper = new Mock<IMapper>();

        _service = new RespostaService(
            _mockRespostaAlternativaRepository.Object,
            _mockMapper.Object,
            _mockInteracaoRepository.Object,
            _mockQuestaoRepository.Object,
            _mockRepository.Object);
    }

    [Fact]
    public async Task Adicionar_DeveAdicionarRespostaAlternativa_QuandoDadosForemValidos()
    {
        // Arrange
        var requestDto = new RespostaRequestDTO(1, 1, 2, null);
        var interacao = new InteracaoModel { Id = 1 };
        var questao = new QuestaoModel { Id = 1 };
        var respostaModel = new RespostaModel(requestDto, interacao, questao);
        
        _mockInteracaoRepository.Setup(x => x.BuscarPorId(requestDto.InteracaoId)).ReturnsAsync(interacao);
        _mockQuestaoRepository.Setup(x => x.BuscarPorId(requestDto.QuestaoId)).ReturnsAsync(questao);
        _mockRespostaAlternativaRepository.Setup(x => x.Adicionar(It.IsAny<RespostaModel>())).ReturnsAsync(respostaModel);
        _mockMapper.Setup(x => x.Map<RespostaResponseDTO>(It.IsAny<RespostaModel>())).Returns(new RespostaResponseDTO(1, 1, 1, 2, null, questao, interacao));

        // Act
        var result = await _service.AdicionarAsync(requestDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(requestDto.QuestaoId, result.QuestaoId);
        Assert.Equal(requestDto.InteracaoId, result.InteracaoId);
        Assert.Equal(requestDto.Alternativa, result.Alternativa);
    }

    [Fact]
    public async Task Adicionar_DeveLancarExcecao_QuandoInteracaoNaoForEncontrada()
    {
        // Arrange
        var requestDto = new RespostaRequestDTO(1, 1, 2, null);
        _mockInteracaoRepository.Setup(x => x.BuscarPorId(requestDto.InteracaoId)).ReturnsAsync((InteracaoModel)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.AdicionarAsync(requestDto));
    }

    [Fact]
    public async Task Adicionar_DeveLancarExcecao_QuandoQuestaoNaoForEncontrada()
    {
        // Arrange
        var requestDto = new RespostaRequestDTO(1, 1, 2, null);
        var interacao = new InteracaoModel { Id = 1 };
        _mockInteracaoRepository.Setup(x => x.BuscarPorId(requestDto.InteracaoId)).ReturnsAsync(interacao);
        _mockQuestaoRepository.Setup(x => x.BuscarPorId(requestDto.QuestaoId)).ReturnsAsync((QuestaoModel)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.AdicionarAsync(requestDto));
    }
}