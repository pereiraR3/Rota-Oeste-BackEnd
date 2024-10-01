using api_rota_oeste.Data;
using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Repositories.Interfaces;
using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Repositories;

public class InteracaoRepository: IInteracaoRepository {

    private readonly ApiDBContext _context;
    private readonly IMapper _mapper;
    private readonly ICheckListRepository? _checkListRepository;
    private readonly IClienteRepository? _clienteRepository;

    // Construtor para injeção de dependência do contexto

    public InteracaoRepository(ApiDBContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper;
    }

    public InteracaoRepository(ApiDBContext context, IMapper mapper, ICheckListRepository checkListRepository, IClienteRepository clienteRepository)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper;
        _checkListRepository = checkListRepository;
        _clienteRepository = clienteRepository;
    }

    public async Task<InteracaoModel> Criar(InteracaoRequestDTO req)
    {
        var cliente = await _clienteRepository.BuscaPorId(req.ClienteId);

        if (cliente == null) throw new Exception("Cliente nao existe");

        var check = await _checkListRepository.FindById(req.CheckListId);

        if (check == null) throw new Exception("Checklist nao existe");

        var interacao = new InteracaoModel(req, cliente, check);

        await _context.AddAsync(interacao);
        await _context.SaveChangesAsync();

        return interacao;
    }

    public void criar(InteracaoModel interacaoModel){
        _context.Interacoes.Add(interacaoModel);
        _context.SaveChanges();
    }
    public async Task<InteracaoModel?> BuscarPorId(int id)
    {

        InteracaoModel? interacao = await _context.Interacoes.FindAsync(id);

        return interacao;
    }

    public async Task<bool> Atualizar(InteracaoPatchDTO req) 
    {
        InteracaoModel? intModel = await BuscarPorId(req.Id);

        if (intModel == null)
            throw new Exception("interacao nao encontrada");

        _mapper.Map(req, intModel);

        _context.Interacoes.Update(intModel);
        await _context.SaveChangesAsync();

        return true;
    }
}