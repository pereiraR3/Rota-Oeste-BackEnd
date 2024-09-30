using api_rota_oeste.Migrations;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Models.Usuario;
using api_rota_oeste.Repositories;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services.Interfaces;
using AutoMapper;

namespace api_rota_oeste.Services;

public class InteracaoService : IInteracaoService {
    
    private readonly IInteracaoRepository _repository;
    private readonly IClienteRepository _clienteRepository;
    private readonly ICheckListRepository _checkListService;
    private readonly IMapper _mapper;
    
    public InteracaoService(IInteracaoRepository repository, IMapper mapper, IClienteRepository clienteRepository, ICheckListRepository checkListRepository)
    {
        _mapper = mapper;
        _repository = repository;
        _clienteRepository = clienteRepository;
        _checkListService = checkListRepository;
    }

    public async void criar(InteracaoRequestDTO interacaoDTO) {
        if (interacaoDTO is null) throw new ArgumentNullException(nameof(interacaoDTO));
        var cliente = await _clienteRepository.BuscaPorId(interacaoDTO.ClienteId);
        var clienteModel = _mapper.Map<ClienteModel>(cliente);
        var interacaoModel = new InteracaoModel();
        interacaoModel.Status = interacaoDTO.Status;
        interacaoModel.Cliente = clienteModel;
        interacaoModel.Data = DateTime.Now;
        _repository.criar(interacaoModel);
    }

    public async Task<InteracaoModel?> BuscarPorId(int id)
    {
        var interacao = _repository.BuscarPorId(id);
        
        if(interacao == null) throw new ArgumentNullException(nameof(id));

        return await interacao;
    }

    public async Task<bool> Atualizar(InteracaoPatchDTO req)
    {
        var result = await _repository.Atualizar(req);

        if (!result) throw new ArgumentException("Erro ao atualizar");

        return result;
    }

}