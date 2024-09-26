using AutoMapper;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.Usuario;
using api_rota_oeste.Models.CheckList;
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {

        CreateMap<ClienteModel, ClienteResponseDTO>();
            
        CreateMap<UsuarioModel, UsuarioResponseDTO>();

        CreateMap<CheckListModel, CheckListResponseDTO>();//
        // O mapeamento de atualização deve ignorar campos nulos
        CreateMap<UsuarioPatchDTO, UsuarioModel>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

    }
}