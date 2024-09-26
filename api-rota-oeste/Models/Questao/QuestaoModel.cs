using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace api_rota_oeste.Models.Questao;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("questao")]
public class QuestaoModel{
    
    [Key]
    public int Id { get; set; }
    
    [StringLength(120)]
    [Required]
    public string titulo { get; set; }
    
    [StringLength(20)]
    [Required]
    public string tipo { get; set; }

    public QuestaoModel(string titulo, string tipo)
    {
        this.titulo = titulo;
        this.tipo = tipo;
    }
}
