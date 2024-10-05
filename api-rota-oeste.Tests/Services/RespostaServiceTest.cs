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
    private readonly Mock<IRespostaRepository> _mockRespostaRepository;
    private readonly Mock<IInteracaoRepository> _mockInteracaoRepository;
    private readonly Mock<IQuestaoRepository> _mockQuestaoRepository;
    private readonly Mock<IRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly RespostaService _service;

    public RespostaServiceTests()
    {
        _mockRespostaRepository = new Mock<IRespostaRepository>();
        _mockInteracaoRepository = new Mock<IInteracaoRepository>();
        _mockQuestaoRepository = new Mock<IQuestaoRepository>();
        _mockRepository = new Mock<IRepository>();
        _mockMapper = new Mock<IMapper>();

        _service = new RespostaService(
            _mockRespostaRepository.Object,
            _mockMapper.Object,
            _mockInteracaoRepository.Object,
            _mockQuestaoRepository.Object,
            _mockRepository.Object,
            new Mock<IRespostaTemAlternativaRepository>().Object,
            new Mock<IAlternativaRepository>().Object
        );
    }

    [Fact]
    public async Task AtualizarAsync_DeveAtualizarResposta_QuandoDadosForemValidos()
    {
        // Arrange
        var respostaPatch = new RespostaPatchDTO(1, new byte[] { 1, 2, 3 });
        var respostaModel = new RespostaModel
        {
            Id = 1,
            QuestaoId = 1,
            InteracaoId = 1,
            Foto = null
        };

        _mockRespostaRepository.Setup(repo => repo.BuscaPorId(respostaPatch.Id))
            .ReturnsAsync(respostaModel);

        // Act
        var result = await _service.AtualizarAsync(respostaPatch);

        // Assert
        Assert.True(result);
        Assert.Equal(respostaPatch.Foto, respostaModel.Foto);
        _mockRespostaRepository.Verify(repo => repo.BuscaPorId(respostaPatch.Id), Times.Once);
        _mockRepository.Verify(repo => repo.Salvar(), Times.Once);
    }

    [Fact]
    public async Task AtualizarAsync_DeveLancarExcecao_QuandoRespostaNaoForEncontrada()
    {
        // Arrange
        var respostaPatch = new RespostaPatchDTO(1, new byte[] { 1, 2, 3 });

        _mockRespostaRepository.Setup(repo => repo.BuscaPorId(respostaPatch.Id))
            .ReturnsAsync((RespostaModel)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.AtualizarAsync(respostaPatch));
    }

    [Fact]
    public async Task AtualizarAsync_DeveManterFotoNula_QuandoFotoNaoForFornecida()
    {
        // Arrange
        var respostaPatch = new RespostaPatchDTO(1, null);
        var respostaModel = new RespostaModel
        {
            Id = 1,
            QuestaoId = 1,
            InteracaoId = 1,
            Foto = new byte[] { 1, 2, 3 }
        };

        _mockRespostaRepository.Setup(repo => repo.BuscaPorId(respostaPatch.Id))
            .ReturnsAsync(respostaModel);

        // Act
        var result = await _service.AtualizarAsync(respostaPatch);

        // Assert
        Assert.True(result);
        Assert.Equal(new byte[] { 1, 2, 3 }, respostaModel.Foto);
        _mockRespostaRepository.Verify(repo => repo.BuscaPorId(respostaPatch.Id), Times.Once);
        _mockRepository.Verify(repo => repo.Salvar(), Times.Once);
    }
}
