using api_rota_oeste.Models.Questao;
using api_rota_oeste.Repositories;
using api_rota_oeste.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace api_rota_oeste.Services;

public class QuestaoService : IQuestaoService{
    private readonly IQuestaoRepository _repository;
    private readonly IMapper _mapper;
    
    public QuestaoService(IQuestaoRepository repository, IMapper mapper)
    {
        _mapper = mapper;
        _repository = repository;
    }
    
    public void criar(QuestaoRequestDTO questao){
        if (questao is null)
        {
            throw new KeyNotFoundException("Questão não pode ser nulo");
        }
        var questaoModel = _mapper.Map<QuestaoModel>(questao);
        _repository.criar(questaoModel);
    }

    public List<QuestaoModel> listar()
    {
        var questoes = _repository.listar();
        if (questoes == null)
        {
            throw new KeyNotFoundException("Não há questões registradas.");
        }
        return questoes;
    }

    public QuestaoResponseDTO obter(int id)
    {
        var questao = _repository.obter(id);
        if (questao == null)
        {
            throw new KeyNotFoundException("Não há questão registrada com o ID informado.");
        }
        var questaoConvertida = _mapper.Map<QuestaoResponseDTO>(questao);
        return questaoConvertida;
    }
    public void editar(int id, QuestaoRequestDTO editar){
        var questaoObtida = _repository.obter(id);
        if (questaoObtida == null)
        {
            throw new KeyNotFoundException("Não há questão registrada com o ID informado.");
        }
        questaoObtida.tipo = editar.tipo;
        questaoObtida.titulo = editar.titulo;
        _repository.salvar();
    }

    public void deletar(int id){
        var questaoObtida = _repository.obter(id);
        if (questaoObtida == null)
        {
            throw new KeyNotFoundException("Não há questão registrada com o ID informado.");
        }
        _repository.deletar(id);
    }
}