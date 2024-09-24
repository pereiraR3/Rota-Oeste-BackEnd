using AutoMapper;
using api_rota_oeste.Models;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.Usuario;
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {

        CreateMap<ClienteModel, ClienteResponseDTO>();
            
        CreateMap<UsuarioModel, UsuarioResponseDTO>();
            
    }
}