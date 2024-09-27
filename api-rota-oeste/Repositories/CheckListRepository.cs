using api_rota_oeste.Data;
using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Usuario;
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

        public async Task<CheckListModel> Add(CheckListRequestDTO req)
        {
            var usuario = await _repository.BuscaPorId(req.UsuarioId);

            if (usuario == null) throw new Exception($"Usuário de id {req.UsuarioId} não foi encontrado");
            
            var check = new CheckListModel(req, usuario);

            await _context.AddAsync(check);
            await _context.SaveChangesAsync(); 

            return check;
        }

        public async Task<CheckListModel?> FindById(int id)
        {
            try
            {
                var check = _context.CheckLists.FirstOrDefault(c => c.Id == id);

                return check;

            }
            catch
            {
                Console.WriteLine($"CheckList de Id {id} não encontrado");
                return null;
            }
        }

        public async Task<List<CheckListModel?>> GetAll()
        {
            List<CheckListModel?> check = await _context.CheckLists.ToListAsync();

            return check;

        }

        public async Task<bool> Delete(int id)
        {
            var check = _context.CheckLists.FirstOrDefault(c => c.Id == id);

            if(check == null) return false;

            _context.CheckLists.Remove(check);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAll()
        {
            List<CheckListModel?> checks = await GetAll();

            if (checks == null) return false;

            foreach(var check in checks)
            {
                _context.CheckLists.Remove(check);
                await _context.SaveChangesAsync(); 
            }

            return true;
        }

        public async Task<List<CheckListModel>> AddCollection(CheckListCollectionDTO req)
        {

            UsuarioModel? usuario = await _repository.BuscaPorId(req.CheckLists.FirstOrDefault()!.UsuarioId);

            List<CheckListModel> checks = new List<CheckListModel>();

            foreach (var check in req.CheckLists)
            {
                CheckListModel checkModel = new CheckListModel(check, usuario);

                _context.Add(checkModel);
                await _context.SaveChangesAsync();

                checks.Add(checkModel);

            }

            return checks.ToList();

        }

    }
}
