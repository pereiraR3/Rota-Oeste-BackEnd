using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services.Interfaces;
using AutoMapper;

namespace api_rota_oeste.Services;

public class InteracaoService : IInteracaoService {
    
    private readonly IInteracaoRepository _repository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IMapper _mapper;
    
    public InteracaoService(IInteracaoRepository repository, IMapper mapper, IClienteRepository clienteRepository)
    {
        _mapper = mapper;
        _repository = repository;
        _clienteRepository = clienteRepository;
    }

    public async void criar(InteracaoRequestDTO interacaoDTO) {
        if (interacaoDTO is null) throw new ArgumentNullException(nameof(interacaoDTO));
        var cliente = await _clienteRepository.BuscaPorId(interacaoDTO.ClienteId);
        var clienteModel = _mapper.Map<ClienteModel>(cliente);
        var interacaoModel = new InteracaoModel();
        interacaoModel.Status = interacaoDTO.Status;
        interacaoModel.cliente = clienteModel;
        interacaoModel.Data = DateTime.Now;
        _repository.criar(interacaoModel);
    }

}