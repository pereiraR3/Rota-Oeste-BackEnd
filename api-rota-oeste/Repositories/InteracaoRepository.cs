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
    private readonly IMapper? _mapper;
    private readonly ICheckListRepository? _checkListRepository;
    private readonly IClienteRepository? _clienteRepository;

    // Construtor para injeção de dependência do contexto

    public InteracaoRepository(ApiDBContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
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
        var cliente = new ClienteModel();

        var check = new CheckListModel();

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

    public async Task<bool> Atualizar(InteracaoModel interacao) 
    {
        InteracaoModel? intModel = await BuscarPorId(interacao.Id);

        if (intModel == null)
            return false;

        _context.Interacoes.Update(intModel);
        await _context.SaveChangesAsync();

        return true;
    }
}