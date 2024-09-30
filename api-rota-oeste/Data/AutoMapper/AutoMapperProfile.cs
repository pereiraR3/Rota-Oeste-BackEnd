using AutoMapper;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Usuario;
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {

        CreateMap<ClienteModel, ClienteResponseDTO>();
            
        CreateMap<UsuarioModel, UsuarioResponseDTO>();
        
        CreateMap<QuestaoModel, QuestaoResponseDTO>();

        CreateMap<CheckListModel, CheckListResponseDTO>();
        
        CreateMap<QuestaoRequestDTO, QuestaoModel>();
        
        CreateMap<InteracaoRequestDTO, InteracaoModel>();
            
        // O mapeamento de atualização deve ignorar campos nulos
        CreateMap<UsuarioPatchDTO, UsuarioModel>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<InteracaoPatchDTO, InteracaoModel>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

    }
    
    
}
