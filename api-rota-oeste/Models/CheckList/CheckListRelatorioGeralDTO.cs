namespace api_rota_oeste.Models.CheckList;

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