using System.Data;
using api_rota_oeste.Data;
using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Repositories.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Dapper;

namespace api_rota_oeste.Repositories
{
    
    /// <summary>
    /// Repositório responsável pelas operações de persistência da entidade CheckList no banco de dados.
    /// </summary>
    /// <remarks>
    /// Implementa a interface <see cref="ICheckListRepository"/> e define métodos para realizar operações CRUD,
    /// além de operações específicas, como a geração de relatórios.
    /// </remarks>
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
        
        /// <summary>
        /// Adiciona uma nova instância da entidade CheckList no banco de dados.
        /// </summary>
        /// <param name="checkList">Objeto que contém os dados do checklist a ser adicionado.</param>
        /// <returns>Retorna o checklist adicionado.</returns>
        public async Task<CheckListModel?> Adicionar(CheckListModel checkList)
        {

            await _context.AddAsync(checkList);
            await _context.SaveChangesAsync(); 

            return checkList;
        }

        /// <summary>
        /// Busca uma instância da entidade CheckList pelo ID.
        /// </summary>
        /// <param name="id">ID do checklist a ser buscado.</param>
        /// <returns>Retorna o checklist correspondente ao ID fornecido, ou null se não for encontrado.</returns>
        public async Task<CheckListModel?> BuscarPorId(int id)
        {

            return await _context.CheckLists.FindAsync(id);

        }

        /// <summary>
        /// Busca todas as instâncias da entidade CheckList armazenadas no banco de dados.
        /// </summary>
        /// <returns>Retorna uma lista de todos os checklists.</returns>
        public async Task<List<CheckListModel>> BuscarTodos()
        {

            return await _context.CheckLists.ToListAsync();

        }

        /// <summary>
        /// Remove uma instância da entidade CheckList pelo ID.
        /// </summary>
        /// <param name="id">ID do checklist a ser removido.</param>
        /// <returns>Retorna true se o checklist for removido com sucesso, caso contrário, retorna false.</returns>
        public async Task<bool> Apagar(int id)
        {
            var check = _context.CheckLists.FirstOrDefault(c => c.Id == id);

            if(check == null) return false;

            _context.CheckLists.Remove(check);

            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Remove todas as instâncias da entidade CheckList armazenadas no banco de dados.
        /// </summary>
        /// <returns>Retorna true se todos os checklists forem removidos com sucesso, caso contrário, retorna false.</returns>
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
        
        /// <summary>
        /// Gera um relatório geral para um checklist específico.
        /// </summary>
        /// <param name="idChecklist">ID do checklist para o qual o relatório deve ser gerado.</param>
        /// <returns>Retorna uma coleção de resultados dinâmicos contendo o relatório gerado.</returns>
        /// <exception cref="Exception">Lança uma exceção em caso de erro ao gerar o relatório.</exception>
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
