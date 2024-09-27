using api_rota_oeste.Models.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_rota_oeste.Models.CheckList
{
    public class CheckListModel
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public DateTime? DataCriacao { get; set; }
        public int UsuarioId { get; set; }
        public virtual UsuarioModel? Usuario { get; set; }

        public CheckListModel() { }
        public CheckListModel(CheckListRequestDTO req, UsuarioModel? usuario)
        {
            this.Nome = req.Nome;
            this.DataCriacao = DateTime.Now; //a dataCriacao do Checklist � definida como a data no momento da criacao
            this.Usuario = usuario;
            this.UsuarioId = req.UsuarioId;
            
        }
    }
}
