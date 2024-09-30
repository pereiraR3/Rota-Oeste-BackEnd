using System;
using System.Threading.Tasks;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Models.Usuario;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services;
using AutoMapper;
using Moq;
using Xunit;

namespace api_rota_oeste.Tests.Services;

public class InteracaoServiceTest
{
    private readonly Mock<IInteracaoRepository> _interacaoRepositoryMock;
    private readonly Mock<IClienteRepository> _clienteRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly InteracaoService _interacaoService;

    public InteracaoServiceTest()
    {
        // Configuração dos mocks necessários
        _interacaoRepositoryMock = new Mock<IInteracaoRepository>();
        _clienteRepositoryMock = new Mock<IClienteRepository>();
        _mapperMock = new Mock<IMapper>();

        // Instanciando a service com os mocks
        _interacaoService = new InteracaoService(_interacaoRepositoryMock.Object, _mapperMock.Object, _clienteRepositoryMock.Object);
    }

    [Fact]
    public async Task criar_DeveAdicionarNovaInteracao()
    {
        // Arrange
        var interacaoDto = new InteracaoRequestDTO(1, 1, true);
        var clienteModel = new ClienteModel { Id = 1, Nome = "Cliente Teste", Telefone = "123456789" };
        var interacaoModel = new InteracaoModel
        {
            Status = true,
            Cliente = clienteModel,
            Data = DateTime.Now
        };

        // Configurar o mock do clienteRepository para retornar um cliente quando BuscaPorId for chamado
        _clienteRepositoryMock.Setup(repo => repo.BuscaPorId(interacaoDto.ClienteId)).ReturnsAsync(clienteModel);

        // Configurar o mock do mapper para mapear ClienteModel para ClienteModel (simular o comportamento do mapeamento)
        _mapperMock.Setup(m => m.Map<ClienteModel>(clienteModel)).Returns(clienteModel);

        // Act
        _interacaoService.criar(interacaoDto);

        // Assert
        _interacaoRepositoryMock.Verify(repo => repo.criar(It.IsAny<InteracaoModel>()), Times.Once);
    }
}
