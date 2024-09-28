using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api_rota_oeste.Models.Cliente;

namespace api_rota_oeste.Models.Interacao;

public class InteracaoModel {
    
    [Key]
    public int Id { get; set; }
    
    [Column(TypeName = "BIT")]
    [Required]
    public bool Status { get; set; }
    
    [Column(TypeName = "DATETIME2")]
    [Required]
    public DateTime Data { get; set; }
    
    [ForeignKey("ClienteId")]
    [Required]
    public ClienteModel cliente { get; set; }
}

