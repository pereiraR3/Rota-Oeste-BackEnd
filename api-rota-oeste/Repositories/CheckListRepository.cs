using api_rota_oeste.Data;
using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Repositories.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api_rota_oeste.Repositories
{
    public class CheckListRepository : ICheckListRepository
    {
        
        private readonly IMapper _mapper;
        private readonly ApiDBContext _context;
        private readonly IUsuarioRepository _repository;

        public CheckListRepository(IMapper mapper, ApiDBContext context, IUsuarioRepository repository)
        {
            _mapper = mapper;
            _context = context;
            _repository = repository;
        }

        public async Task<CheckListModel?> Adicionar(CheckListModel checkList)
        {

            await _context.AddAsync(checkList);
            await _context.SaveChangesAsync(); 

            return checkList;
        }

        public async Task<CheckListModel?> BuscarPorId(int id)
        {
            
            return await _context.CheckLists.FindAsync(id);

        }

        public async Task<List<CheckListModel>> BuscarTodos()
        {

            return await _context.CheckLists.ToListAsync();

        }

        public async Task<bool> Apagar(int id)
        {
            var check = _context.CheckLists.FirstOrDefault(c => c.Id == id);

            if(check == null) return false;

            _context.CheckLists.Remove(check);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ApagarTodos()
        {
            List<CheckListModel?> checks = await BuscarTodos();

            if (checks == null) return false;

            foreach(var check in checks)
            {
                _context.CheckLists.Remove(check);
                await _context.SaveChangesAsync(); 
            }

            return true;
        }

    }
}
