﻿using api_rota_oeste.Data;
using api_rota_oeste.Data;
using api_rota_oeste.Repositories;
using AutoMapper;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using api_rota_oeste.Models.Questao;

namespace api_rota_oeste.Tests.Repositories;

public class QuestaoRepositoryTest
{
    private readonly ApiDBContext _dbContext;
    private readonly IMapper _mapper;
    private readonly QuestaoRepository _questaoRepository;

    public QuestaoRepositoryTest()
    {
        // Configuração do DbContext para usar o InMemoryDatabase
        var options = new DbContextOptionsBuilder<ApiDBContext>()
            .UseInMemoryDatabase(databaseName: "ApiRotaOesteTestDB")
            .Options;

        _dbContext = new ApiDBContext(options);

        // Configurar AutoMapper (opcional: pode ser um mock ou a configuração real)
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<QuestaoRequestDTO, QuestaoModel>()
                .ForAllMembers(ops => ops.Condition((src, dest, srcMember) => srcMember != null));
        });

        // Inicializando o repositório
        _questaoRepository = new QuestaoRepository(_dbContext);
    }
    
    [Fact]
    public async Task AdicionarQuestao()
    {
        // Arrange: Criar a questão com dados fictícios
        var questao = new QuestaoModel(
            "tituloteste",
            "tipoteste"
        );

        // Act: Adicionar a questão ao banco de dados em memória
        _questaoRepository.criar(questao);

        // Verifica se a questão foi persistida corretamente no banco de dados
        var questaoNoBanco = await _dbContext.Questoes.FirstOrDefaultAsync(u => u.Titulo == "tituloteste" && u.Tipo == "tipoteste");
        Assert.NotNull(questaoNoBanco);
        Assert.Equal("tituloteste", questaoNoBanco.Titulo);
    }
    
    [Fact]
    public async Task BuscarPorId_DeveRetornarQuestaoModel()
    {
            
        // Arrange: Criar a questão com dados fictícios
        var questao = new QuestaoModel(
            "tituloteste", 
            "tipoteste"
        );
            
        // Act: Adicionar a questão ao banco de dados em memória
        _questaoRepository.criar(questao);
        var questaoModel = _questaoRepository.obter(1);
            
        //Assert: verifica se o retorno é o esperado
        Assert.NotNull(questaoModel);
        Assert.Equal(questao.Titulo, questaoModel.Titulo);
        Assert.Equal(questao.Tipo, questaoModel.Tipo);
    }
    
    [Fact]
    public async Task BuscarPorId_DeveRetornarNotFound()
    {
            
        // Arrange: Criar o request DTO com dados fictícios
            
        /*
         * Neste caso não haverá dados, pois vamos forçar um Not Found
         */
            
        // Act: Adicionar o usuário ao banco de dados em memória
        var questaoModel = _questaoRepository.obter(1);
            
        // Assert: verifica se o retorno é o esperado
        Assert.Null(questaoModel);
    }
    
    [Fact]
    public async Task buscarTodos_deveRetornarTodasQuestoes()
    {
            
        // Arrange: Criar a questão com dados fictícios
        var questao1 = new QuestaoModel(
            "tituloteste", 
            "tipoteste"
        );
        
        var questao2 = new QuestaoModel(
            "tituloteste", 
            "tipoteste"
        );
            
        // Act: Adicionar a questão ao banco de dados em memória
        _questaoRepository.criar(questao1);
        _questaoRepository.criar(questao2);
        var questoes = _questaoRepository.listar();
            
        //Assert: verifica se o retorno é o esperado
        Assert.NotNull(questoes);
        Assert.Equal(questoes[0].Titulo, questao1.Titulo);
        Assert.Equal(questoes[1].Titulo, questao2.Titulo);
    }
    
    [Fact]
    public async Task ApagarQuestao()
    {
        // Arrange: Criar a questão com dados fictícios
        var questao1 = new QuestaoModel(
            "tituloteste", 
            "tipoteste"
        );

        // Act: Adicionar o usuário ao banco de dados em memória e removê-lo
        _questaoRepository.criar(questao1);
        _questaoRepository.deletar(1);
        var questaoBanco = await _dbContext.Questoes.FirstOrDefaultAsync(u => u.Titulo == "tituloteste");

        // Assert: verifica se o retorno é o esperado
        Assert.Null(questaoBanco);
    }
}