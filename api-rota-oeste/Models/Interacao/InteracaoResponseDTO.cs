using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_rota_oeste.Models.Interacao
{
    public record InteracaoResponseDTO
    (
        int Id,
        int ClienteId,
        int CheckListId,
        DateTime DataCriacao,
        bool Status
    );
}