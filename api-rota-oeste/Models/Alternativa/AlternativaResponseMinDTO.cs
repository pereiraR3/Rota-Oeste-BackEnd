namespace api_rota_oeste.Models.Alternativa;

public record AlternativaResponseMinDTO
{
    public int Id { get; set; }
    
    public int QuestaoId { get; set; }
    
    public string Descricao { get; set; }
    
    public int Codigo { get; set; }
    
    public AlternativaResponseMinDTO(int id, int questaoId, string descricao, int codigo )
    {
        Id = id;
        
        QuestaoId = questaoId;
        
        Descricao = descricao;
        
        Codigo = codigo;
        
    }
    
}