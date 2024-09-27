using AutoMapper;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Models.Usuario;
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {

        CreateMap<ClienteModel, ClienteResponseDTO>();
            
        CreateMap<UsuarioModel, UsuarioResponseDTO>();
        
        CreateMap<QuestaoModel, QuestaoResponseDTO>();
        
        CreateMap<QuestaoRequestDTO, QuestaoModel>();
            
        // O mapeamento de atualização deve ignorar campos nulos
        CreateMap<UsuarioPatchDTO, UsuarioModel>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

    }
    
    
}