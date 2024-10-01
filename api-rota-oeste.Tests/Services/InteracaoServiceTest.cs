using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Models.Usuario;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services;
using api_rota_oeste.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity.Data;
using Moq;
using Xunit;

namespace api_rota_oeste.Tests.Services;

public class InteracaoServiceTest
{
    private readonly Mock<IInteracaoRepository> _interacaoRepositoryMock;
    private readonly Mock<IClienteRepository> _clienteRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICheckListRepository> _checkListRepositoryMock;
    private readonly InteracaoService _interacaoService;

    public InteracaoServiceTest()
    {
        // Configuração dos mocks necessários
        _interacaoRepositoryMock = new Mock<IInteracaoRepository>();
        _clienteRepositoryMock = new Mock<IClienteRepository>();
        _mapperMock = new Mock<IMapper>();
        _checkListRepositoryMock = new Mock<ICheckListRepository>();

        // Instanciando a service com os mocks
        _interacaoService = new InteracaoService(_interacaoRepositoryMock.Object, _mapperMock.Object, _clienteRepositoryMock.Object, _checkListRepositoryMock.Object);
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

    [Fact]
    public async Task BuscarPorId()
    {
        var usuarioModel = new UsuarioModel { Id = 1, Nome = "Cliente Teste", Telefone = "123456789" };
        var clienteModel = new ClienteModel { Id = 1, Nome = "Cliente Teste", Telefone = "123456789" };
        var checkModel = new CheckListModel { Id = 1, Nome = "Cliente Teste", Usuario = usuarioModel };
        var intModel = new InteracaoModel
        {
            Id = 1,
            Status = true,
            Cliente = clienteModel,
            Data = DateTime.Now,
        };

        var intResponse = new InteracaoResponseDTO
        (
            1,
            1,
            1,
            DateTime.Now,
            true
        );

        _interacaoRepositoryMock.Setup(repo => repo.BuscarPorId(It.IsAny<int>()))
            .ReturnsAsync(intModel);

        _mapperMock.Setup(mapper => mapper.Map<InteracaoResponseDTO>(intModel))
            .Returns(intResponse);

        // Act
        var result = await _interacaoService.BuscarPorId(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(intResponse.Id, result.Id);
    }

    [Fact]
    public async Task Atualizar()
    {
        var usuarioModel = new UsuarioModel { Id = 1, Nome = "Cliente Teste", Telefone = "123456789" };
        var clienteModel = new ClienteModel { Id = 1, Nome = "Cliente Teste", Telefone = "123456789" };
        var checkModel = new CheckListModel { Id = 1, Nome = "Cliente Teste", Usuario = usuarioModel };
        var intModel = new InteracaoModel
        {
            Id = 1,
            Status = true,
            Cliente = clienteModel,
            Data = DateTime.Now,
        };

        var intPatch = new InteracaoPatchDTO
        { 
            Id = 1,
            Status = true,
            Data = DateTime.Now
        };


        _interacaoRepositoryMock.Setup(repo => repo.Atualizar(It.IsAny<InteracaoPatchDTO>()))
                .ReturnsAsync(true);

        // Act
        var result = await _interacaoService.Atualizar(intPatch);

        // Assert
        Assert.True(result);

    }

}

    
