using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace api_rota_oeste.Models.Interacao;

public record InteracaoRequestDTO(
    int ClienteId,
    
    bool Status,
    
    string Telefone
);
