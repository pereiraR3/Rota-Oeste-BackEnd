using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.ClienteRespondeCheckList;
using api_rota_oeste.Models.Questao;
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
        private readonly IClienteRespondeCheckListRepository _repositoryClienteRespondeCheck;
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IClienteRepository _repositoryCliente;


        public CheckListService(
            
            ICheckListRepository repositoryCheckList,
            IUsuarioRepository repositoryUsuario,
            IMapper mapper, IRepository repository,
            IClienteRespondeCheckListRepository repositoryClienteRespondeCheck,
            IClienteRepository repositoryCliente
            
            )
        {
            _repositoryCheckList = repositoryCheckList;
            _repositoryUsuario = repositoryUsuario;
            _mapper = mapper;
            _repository = repository;
            _repositoryClienteRespondeCheck = repositoryClienteRespondeCheck;
            _repositoryCliente = repositoryCliente;
        }

        /**
        * Método da camada de servico -> para criar uma entidade do tipo checklist
        */
        public async Task<CheckListResponseDTO?> AdicionarAsync(CheckListRequestDTO checkListRequestDto)
        {
            UsuarioModel? usuarioModel = await _repositoryUsuario.BuscaPorId(checkListRequestDto.UsuarioId);

            if (usuarioModel == null)
                throw new KeyNotFoundException("Usuário não encontrado");
            
            CheckListModel checkListModel = new CheckListModel(checkListRequestDto, usuarioModel);
            
            var check = await _repositoryCheckList.Adicionar(checkListModel);

            if (check != null)
            {
                usuarioModel.CheckLists.Add(check);

                return _mapper.Map<CheckListResponseDTO>(check);
            }
            
            return null;
        }

        public async Task<ClienteRespondeCheckListResponseDTO> AdicionarClienteRespondeCheckAsync(int clienteId, int checkListId)
        {

            ClienteModel? clienteModel = await _repositoryCliente.BuscarPorId(clienteId);

            if (clienteModel == null)
                throw new KeyNotFoundException("Cliente não encontrado");
            
            CheckListModel? checkListModel = await _repositoryCheckList.BuscarPorId(checkListId);
            
            if (checkListModel == null)
                throw new KeyNotFoundException("CheckList não encontrado");
            
            ClienteRespondeCheckListModel clienteResponde = new ClienteRespondeCheckListModel(
                
                clienteId,
                checkListId,
                clienteModel,
                checkListModel
                
                );
            
            ClienteRespondeCheckListModel clienteRespondeCheckListModel = await _repositoryClienteRespondeCheck.Adicionar(clienteResponde);

            // Restringindo a navegabilidade das entidades associadas à ClienteRespondeCheckList
            clienteRespondeCheckListModel = RefatoraoMinClienteRespondeCheckList(clienteRespondeCheckListModel);
            
            return _mapper.Map<ClienteRespondeCheckListResponseDTO>(clienteRespondeCheckListModel);
            
        }

        /**
         * Método da camada de servico -> para buscar determinada entidade checklist por Id
         */
        public async Task<CheckListResponseDTO?> BuscarPorIdAsync(int id)
        {
            
            if(id <= 0)
                throw new ArgumentException("O ID deve ser maior que zero.", nameof(id));
            
            CheckListModel? check = await _repositoryCheckList.BuscarPorId(id);

            if (check == null)
                throw new KeyNotFoundException("Entidade checklist não encontrada");

            // Restrigindo a navegabilidade
            check = RefatoraoMinCheckListModel(check);
            
            return _mapper.Map<CheckListResponseDTO>(check);
        }

        /**
        * Método da camada de servico -> para buscar todas as entidades do tipo checklist
        */
        public async Task<List<CheckListResponseDTO>> BuscarTodosAsync()
        {

            var checks = await _repositoryCheckList.BuscarTodos();

            List<CheckListResponseDTO> checklists = checks
                .Select(i => _mapper.Map<CheckListResponseDTO>(i))
                .ToList();

            return checklists;
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
       * Método da camada de servico -> para apagar uma determinada entidade ClienteRespondeCheckList via ID
       */
        public async Task<bool> ApagarClienteRespondeCheckAsync(int clienteId, int checkListId)
        {
            
            var resultado = await _repositoryClienteRespondeCheck.Apagar(clienteId, checkListId);
            
            if(!resultado)
                throw new ApplicationException("Operação não realizada");
            
            return resultado;
            
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
        
        /**
         * Método da camada de serviço -> para fazer a refatoracao do DTO da entidade ClienteResponde,
         * de modo que puxem apenas as informações que foram julgadas como necessárias
         */
        public ClienteRespondeCheckListModel RefatoraoMinClienteRespondeCheckList(
            
            ClienteRespondeCheckListModel clienteRespondeCheckListModel
            
            )
        {
            
            clienteRespondeCheckListModel.Cliente = new ClienteModel
            {
                Id = clienteRespondeCheckListModel.Cliente.Id,
                Nome = clienteRespondeCheckListModel.Cliente.Nome,
                Telefone = clienteRespondeCheckListModel.Cliente.Telefone,
                Foto = clienteRespondeCheckListModel.Cliente.Foto
                // Não incluímos as outras relações, como Interacoes ou CheckLists
            };
            
            clienteRespondeCheckListModel.CheckList = new CheckListModel
            {
                Id = clienteRespondeCheckListModel.CheckList.Id,
                Nome = clienteRespondeCheckListModel.CheckList.Nome,
                DataCriacao = clienteRespondeCheckListModel.CheckList.DataCriacao
                // Não incluímos as outras relações, como Questoes ou Clientes
            };
            
            return clienteRespondeCheckListModel;
            
        }

        /**
        * Método da camada de serviço -> para fazer a refatoracao do DTO da entidade CheckList,
         * de modo que puxem apenas as informações que foram julgadas como necessárias
         */
        public CheckListModel RefatoraoMinCheckListModel(CheckListModel checkListModel)
        {

            checkListModel.Usuario = new UsuarioModel
            {
                Id = checkListModel.Usuario.Id,
                Nome = checkListModel.Usuario.Nome,
                Telefone = checkListModel.Usuario.Telefone
            };

            // Verifique se existem questões antes de tentar mapeá-las
            if (checkListModel.Questoes != null && checkListModel.Questoes.Any())
            {
                checkListModel.Questoes = checkListModel.Questoes
                    .Select(o => new QuestaoModel
                    {
                        Id = o.Id,
                        CheckListId = o.CheckListId,
                        Tipo = o.Tipo,
                        Titulo = o.Titulo
                    }).ToList();
            }

            // Verifique se existem ClienteRespondeCheckLists antes de tentar mapeá-las
            if (checkListModel.ClienteRespondeCheckLists != null && checkListModel.ClienteRespondeCheckLists.Any())
            {
                checkListModel.ClienteRespondeCheckLists = checkListModel.ClienteRespondeCheckLists
                    .Select(o => new ClienteRespondeCheckListModel
                    {
                        ClienteId = o.ClienteId,
                        CheckListId = o.CheckListId,
                        Cliente = o.Cliente != null ? new ClienteModel
                        {
                            Id = o.Cliente.Id,
                            UsuarioId = o.Cliente.UsuarioId,
                            Nome = o.Cliente.Nome,
                            Telefone = o.Cliente.Telefone
                        } : null
                    }).ToList();
            }

            return checkListModel;
        }


    }
}
