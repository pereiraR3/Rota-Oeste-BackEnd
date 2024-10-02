using AutoMapper;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.ClienteRespondeCheckList;
using api_rota_oeste.Models.RespostaAlternativa;
using api_rota_oeste.Models.Usuario;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        
        // Mapeando a Entidade ClienteRespondeCheckList 
        
            // -> mapeamento incluindo as entidades de navegação
            CreateMap<ClienteRespondeCheckListModel, ClienteRespondeCheckListResponseDTO>()
                .ForMember(dest => dest.CheckList, opt => opt.MapFrom(src => src.CheckList))
                .ForMember(dest => dest.Cliente, opt => opt.MapFrom(src => src.Cliente));
            
        // Mapeando a Entidade Cliente 

            // -> mapeamento incluindo as entidades de navegação
            CreateMap<ClienteModel, ClienteResponseDTO>()
                .ForMember(dest => dest.Usuario, opt => opt.Ignore())
                .ForMember(dest => dest.Interacoes, opt => opt.MapFrom(src => src.Interacoes))
                .ForMember(dest => dest.ClienteResponde, opt => opt.MapFrom(src => src.ClienteRespondeCheckLists));

        // Mapeando a Entidade Questao
        
            CreateMap<QuestaoRequestDTO, QuestaoModel>();

            // -> mapeamento incluindo as entidades de navegação
            CreateMap<QuestaoModel, QuestaoResponseDTO>()
                .ForMember(dest => dest.CheckList, opt => opt.MapFrom(src => src.CheckList))
                .ForMember(dest => dest.RespostaAlternativaModels, opt => opt.MapFrom(src => src.RespostaAlternativaModels));
            
            // -> O mapeamento de atualização deve ignorar campos nulos
            CreateMap<QuestaoPatchDTO, QuestaoModel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        //  Mapeando a Entidade CheckList
            
            CreateMap<CheckListModel, CheckListResponseDTO>()
                .ForMember(dest => dest.Questoes, opt => opt.MapFrom(src => src.Questoes))
                .ForMember(dest => dest.Usuario, opt => opt.Ignore())
                .ForMember(dest => dest.ClienteResponde, opt => opt.MapFrom(src => src.ClienteRespondeCheckLists));

            // -> O mapeamento de atualização deve ignorar campos nulos
            CreateMap<CheckListPatchDTO, CheckListModel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            
        // Mapeando a Entidade Interacao

        CreateMap<InteracaoRequestDTO, InteracaoModel>();
        
            // -> mapeamento incluindo as entidades de navegação
            CreateMap<InteracaoModel, InteracaoResponseDTO>()
                .ForMember(dest => dest.CheckList, opt => opt.MapFrom(src => src.CheckList))
                .ForMember(dest => dest.Cliente, opt => opt.MapFrom(src => src.Cliente))
                .ForMember(dest => dest.RespostaAlternativaModels, opt => opt.MapFrom(src => src.RespostaAlternativaModels));
            
            // -> O mapeamento de atualização deve ignorar campos nulos
            CreateMap<InteracaoPatchDTO, InteracaoModel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            
        // Mapeando a Entidade Usuário
        
            // -> mapeamento incluindo as entidades de navegação
            CreateMap<UsuarioModel, UsuarioResponseDTO>()
                .ForMember(dest => dest.CheckLists, opt => opt.MapFrom(src => src.CheckLists))
                .ForMember(dest => dest.Clientes, opt => opt.MapFrom(src => src.Clientes));
            
            // -> mapeamento de atualização deve ignorar campos nulos
            CreateMap<UsuarioPatchDTO, UsuarioModel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            
        // Mapeando a Entidade RespostaAlternativa
        
            // -> mapeamento incluindo as entidades de navegação
            CreateMap<RespostaAlternativaModel, RespostaAlternativaResponseDTO>()
                .ForMember(dest => dest.Interacao, opt => opt.MapFrom(src => src.Interacao))
                .ForMember(dest => dest.Questao, opt => opt.MapFrom(src => src.Questao));

            // -> mapeamento de atualização deve ignorar campos nulos
            CreateMap<RespostaAlternativaPatchDTO, RespostaAlternativaModel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            
    }
    
    
}
