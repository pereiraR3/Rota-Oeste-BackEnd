using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Usuario;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services.Interfaces;
using AutoMapper;

namespace api_rota_oeste.Services
{
    
    /**
     * Representa a camada de serviço, isto é, a camada onde fica as regras de negócio da aplicação
     */
    public class CheckListService : ICheckListService
    {
        private readonly ICheckListRepository _repositoryCheckList;
        private readonly IUsuarioRepository _repositoryUsuario;
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public CheckListService(
            
            ICheckListRepository repositoryCheckList,
            IUsuarioRepository repositoryUsuario,
            IMapper mapper, IRepository repository)
        {
            _repositoryCheckList = repositoryCheckList;
            _repositoryUsuario = repositoryUsuario;
            _mapper = mapper;
            _repository = repository;
        }

        /**
        * Método da camada de servico -> para criar uma entidade do tipo checklist
        */
        public async Task<CheckListResponseDTO> AdicionarAsync(CheckListRequestDTO checkListRequestDto)
        {
            UsuarioModel? usuarioModel = await _repositoryUsuario.BuscaPorId(checkListRequestDto.UsuarioId);

            if (usuarioModel == null)
                throw new KeyNotFoundException("Usuário não encontrado");
            
            CheckListModel checkListModel = new CheckListModel(checkListRequestDto, usuarioModel);
            
            var check = await _repositoryCheckList.Adicionar(checkListModel);

            usuarioModel.CheckLists.Add(check);
            
            return _mapper.Map<CheckListResponseDTO>(check);
        }

        /**
         * Método da camada de servico -> para buscar determinada entidade checklist por Id
         */
        public async Task<CheckListResponseDTO?> BuscarPorIdAsync(int id)
        {
            
            if(id <= 0)
                throw new ArgumentException("O ID deve ser maior que zero.", nameof(id));
            
            var check = await _repositoryCheckList.BuscarPorId(id);

            if (check == null)
                throw new KeyNotFoundException("Entidade checklist não encontrada");

            return _mapper.Map<CheckListResponseDTO>(check);
        }

        /**
        * Método da camada de servico -> para buscar todas as entidades do tipo checklist
        */
        public async Task<List<CheckListResponseDTO>> BuscarTodosAsync()
        {

            List<CheckListModel?> checks = await _repositoryCheckList.BuscarTodos();

            if (checks == null || !checks.Any())
                throw new InvalidOperationException("Conteúdo não encontrado");

            List<CheckListResponseDTO> resp = checks
                .Select(i => _mapper.Map<CheckListResponseDTO>(i))
                .ToList();

            return resp;
        }

        /**
        * Método da camada de servico -> para atualizar parcialmente uma entidade do tipo checklist
        */
        public async Task<bool> AtualizarAsync(CheckListPatchDTO checkListPatchDto)
        {

            CheckListModel? checkListModel = await _repositoryCheckList.BuscarPorId(checkListPatchDto.Id);
            
            if(checkListModel == null)
                throw new KeyNotFoundException("CheckList não encontrado");
            
            // O mapeamento de atualização deve ignorar campos nulos
            _mapper.Map(checkListPatchDto, checkListModel);

            _repository.Salvar();
            
            return true;

        }

        /**
         * Método da camada de servico -> para apagar um entidade checklist por Id
         */
        public async Task<bool> ApagarAsync(int id)
        {
            if(id <= 0)
                throw new ArgumentException("O ID deve ser maior que zero.", nameof(id));
            
            var result = await _repositoryCheckList.Apagar(id);

            if (!result)
                throw new KeyNotFoundException("CheckList não encontrado");

            return true;
        }
        
        /**
         * Método da camada de servico -> para apagar todas as entidades do tipo checklist
         */
        public async Task<bool> ApagarTodosAsync()
        {
            var result = await _repositoryCheckList.ApagarTodos();

            if (!result)
                throw new ApplicationException("Operação não foi realizada");

            return true;

        }

    }
}
