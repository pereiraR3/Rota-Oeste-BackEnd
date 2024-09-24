using api_rota_oeste.Models.Usuario;

namespace api_rota_oeste.Models.Cliente;

public class ClienteModel
{
    public int Id { get; set; }
    
    public int UsuarioId { get; set; }
    
    public virtual UsuarioModel Usuario { get; set; }
    
    public string Nome { get; set; }
    
    public string Telefone { get; set; }
    
    public byte[]? Foto { get; set; }
    public ClienteModel(){}

    public ClienteModel(ClienteRequestDTO request, UsuarioModel usuario)
    {
        this.UsuarioId = request.UsuarioId;
        this.Usuario = usuario;
        this.Nome = request.Nome;
        this.Telefone = request.Telefone;
        this.Foto = request.Foto;
    }
    
}
