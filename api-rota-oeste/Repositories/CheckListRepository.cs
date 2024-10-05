using System.Data;
using api_rota_oeste.Data;
using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Repositories.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Dapper;

namespace api_rota_oeste.Repositories
{
    public class CheckListRepository : ICheckListRepository
    {
        
        private readonly IMapper _mapper;
        private readonly ApiDbContext _context;
        private readonly IUsuarioRepository _repository;

        public CheckListRepository(IMapper mapper, ApiDbContext context, IUsuarioRepository repository)
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
        
        public async Task<IEnumerable<dynamic>> GerarRelatorioGeral(int idChecklist)
        {
            try
            {
                using (var connection = _context.Database.GetDbConnection())
                {
                    if (connection.State == ConnectionState.Closed)
                        await connection.OpenAsync();

                    var query = "SELECT * FROM dbo.fn_relatorio_geral_checklist(@IdCheckList)";
                    var result = await connection.QueryAsync(query, new { IdCheckList = idChecklist });
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
                throw; // Relance a exceção para tratamento adicional se necessário
            }
        }

    }
}
