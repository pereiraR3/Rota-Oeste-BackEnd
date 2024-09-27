using api_rota_oeste.Data;
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Repositories;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http.HttpResults;

public class QuestaoRepository : IQuestaoRepository
{
    private readonly ApiDBContext _context;

    // Construtor para injeção de dependência do contexto
    public QuestaoRepository(ApiDBContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void criar(QuestaoModel questao)
    {
        Console.WriteLine($"Criando questao: {questao.titulo} {questao.tipo}");
        _context.Questoes.Add(questao);
        _context.SaveChanges();
    }

    public List<QuestaoModel> listar()
    {
        return _context.Questoes.ToList();
    }

    public QuestaoModel obter(int id)
    {
        var questao = _context.Questoes.Find(id);
        return questao;
    }

    public void salvar()
    {
        _context.SaveChanges();
    }

    public void deletar(int id) {
        _context.Questoes.Remove(obter(id));
        salvar();
    }
}