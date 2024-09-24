using AutoMapper;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.Usuario;
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {

        CreateMap<ClienteModel, ClienteResponseDTO>();
            
        CreateMap<UsuarioModel, UsuarioResponseDTO>();
            
        // O mapeamento de atualização deve ignorar campos nulos
        CreateMap<UsuarioPatchDTO, UsuarioModel>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

    }
}