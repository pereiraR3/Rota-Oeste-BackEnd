using System.Text.Json.Serialization;
using api_rota_oeste.Models.Cliente;

namespace api_rota_oeste.Models.Usuario
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        
        public string Telefone { get; set; }
        
        public string Nome { get; set; }
        
        [JsonIgnore]
        public string Senha { get; set; }
        public byte[]? Foto { get; set; }
        
        [JsonIgnore]
        public List<ClienteModel> Clientes { get; set; }

        public UsuarioModel(){}
        
        public UsuarioModel(UsuarioRequestDTO request)
        {
            this.Telefone = request.Telefone;
            this.Nome = request.Nome;
            this.Senha = request.Senha;
            this.Foto = request.Foto;
            this.Clientes = new List<ClienteModel>();
        }
        
    }
}