namespace api_rota_oeste.Models.RespostaAlternativa;

public class RespostaAlternativaModel
{
        public int Id { get; set; }
        
        public int QuestaoId { get; set; }
        
        // public virtual Questao Questao { get; set; }
        
        public int InteracaoId { get; set; }
        
        // public virtual Interacao Interacao { get; set; }
        
        public int Alternativa { get; set; }
        public RespostaAlternativaModel(RespostaAlternativaRequestDTO request)
        {
                
                this.QuestaoId = request.QuestaoId;
                this.InteracaoId = request.InteracaoId;
                this.Alternativa = request.Alternativa;
        }
        
        
}
