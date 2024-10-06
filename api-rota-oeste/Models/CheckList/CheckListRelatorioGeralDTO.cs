namespace api_rota_oeste.Models.CheckList;

/// <summary>
/// Representa o DTO <see cref="CheckListRelatorioGeralDTO"/> que descreve os dados de um relatório geral do checklist, incluindo interações e respostas.
/// </summary>
/// <remarks>
/// Este DTO é utilizado para puxar informações detalhadas de um checklist, incluindo o cliente, questão, respostas e alternativas associadas.
/// </remarks>
/// <example>
/// Aqui está um exemplo de como usar o <see cref="CheckListRelatorioGeralDTO"/>:
/// <code>
/// var relatorioGeral = new CheckListRelatorioGeralDTO(1, "Nome do Cliente", "Nome do Checklist", DateTime.Now, "Título da Questão", 1, 2);
/// </code>
/// </example>
/// <seealso cref="CheckListModel"/>
public record CheckListRelatorioGeralDTO {
    
    public int Id_interacao { get; set; }
    public string Nome_cliente { get; set; }
    public string Nome_checklist { get; set; }
    public DateTime Data_interacao { get; set; }
    public string questao { get; set; }
    public int? Id_resposta { get; set; }
    public int? alternativa { get; set; }

    public CheckListRelatorioGeralDTO() {}

    public CheckListRelatorioGeralDTO(int idInteracao, string nomeCliente, string nomeChecklist, DateTime dataInteracao, string questao, int? idResposta, int? alternativa)
    {
        Id_interacao = idInteracao;
        Nome_cliente = nomeCliente;
        Nome_checklist = nomeChecklist;
        Data_interacao = dataInteracao;
        this.questao = questao;
        Id_resposta = idResposta;
        this.alternativa = alternativa;
    }
}