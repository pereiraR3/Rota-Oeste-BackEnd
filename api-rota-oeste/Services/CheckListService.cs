using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Cliente;
using api_rota_oeste.Models.ClienteRespondeCheckList;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Models.Questao;
using api_rota_oeste.Models.Usuario;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services.Interfaces;
using AutoMapper;

namespace api_rota_oeste.Services
{
    
    /// <summary>
    /// Serviço responsável pelas operações de lógica de negócio relacionadas à entidade CheckList.
    /// </summary>
    /// <remarks>
    /// Implementa a interface <see cref="ICheckListService"/> e define métodos para adicionar, buscar, atualizar e apagar entidades do tipo CheckList.
    /// Além disso, gerencia relacionamentos com outras entidades como Cliente, Usuario, e Interacao.
    /// </remarks>
    public class CheckListService : ICheckListService
    {
        private readonly ICheckListRepository _repositoryCheckList;
        private readonly IUsuarioRepository _repositoryUsuario;
        private readonly IClienteRespondeCheckListRepository _repositoryClienteRespondeCheck;
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IClienteRepository _repositoryCliente;
        private readonly IInteracaoRepository _repositoryInteracao;
        private readonly ILogger<CheckListService> _logger;

        public CheckListService(
            
            ICheckListRepository repositoryCheckList,
            IUsuarioRepository repositoryUsuario,
            IMapper mapper, IRepository repository,
            IClienteRespondeCheckListRepository repositoryClienteRespondeCheck,
            IClienteRepository repositoryCliente,
            IInteracaoRepository repositoryInteracao,
            ILogger<CheckListService> logger
            )
        {
            _repositoryCheckList = repositoryCheckList;
            _repositoryUsuario = repositoryUsuario;
            _mapper = mapper;
            _repository = repository;
            _repositoryClienteRespondeCheck = repositoryClienteRespondeCheck;
            _repositoryCliente = repositoryCliente;
            _repositoryInteracao = repositoryInteracao;
            _logger = logger;
        }

        /// <summary>
        /// Cria uma nova entidade do tipo CheckList e a adiciona ao banco de dados.
        /// </summary>
        /// <param name="checkListRequestDto">Objeto contendo os dados do checklist a ser criado.</param>
        /// <returns>Retorna o DTO de resposta contendo as informações do checklist criado.</returns>
        /// <exception cref="KeyNotFoundException">Lançada se o usuário associado ao checklist não for encontrado.</exception>
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

                check = RefatoraoMinCheckListModel(check);
                
                return _mapper.Map<CheckListResponseDTO>(check);
            }
            
            return null;
        }

        /// <summary>
        /// Cria uma nova relação entre Cliente e CheckList e adiciona uma entidade de interação automaticamente.
        /// </summary>
        /// <param name="clienteId">ID do cliente associado.</param>
        /// <param name="checkListId">ID do checklist associado.</param>
        /// <returns>Retorna o DTO de resposta contendo as informações da relação Cliente-CheckList criada.</returns>
        /// <exception cref="KeyNotFoundException">Lançada se o cliente ou o checklist não for encontrado.</exception>
        /// <exception cref="InvalidOperationException">Lançada se ocorrer um erro ao adicionar ClienteRespondeCheckList ou Interacao.</exception>
        public async Task<ClienteRespondeCheckListResponseDTO> AdicionarClienteRespondeCheckAsync(int clienteId, int checkListId)
        {
            try
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
                
                ClienteRespondeCheckListModel? clienteRespondeCheckListModel = await _repositoryClienteRespondeCheck.Adicionar(clienteResponde);

                if (clienteRespondeCheckListModel == null)
                    throw new InvalidOperationException("Erro ao adicionar ClienteRespondeCheckList");

                // Criar uma entidade do tipo Interacao após adicionar ClienteRespondeCheckList
                InteracaoModel interacaoModel = new InteracaoModel
                {
                    Cliente = clienteModel,
                    CheckList = checkListModel,
                    Status = false, // Defina o status inicial conforme necessário
                    Data = DateTime.Now // Defina a data de criação
                };
                
                InteracaoModel? interacaoCriada = await _repositoryInteracao.Adicionar(interacaoModel);
                if (interacaoCriada == null)
                    throw new InvalidOperationException("Erro ao adicionar Interacao automaticamente");

                // Restringir a navegabilidade das entidades associadas a ClienteRespondeCheckList
                clienteRespondeCheckListModel = RefatoraoMinClienteRespondeCheckList(clienteRespondeCheckListModel);
                
                return _mapper.Map<ClienteRespondeCheckListResponseDTO>(clienteRespondeCheckListModel);
            }
            catch (Exception ex)
            {
                // Logar o erro para análise posterior
                _logger.LogError(ex, "Erro ao adicionar ClienteRespondeCheck");
                throw;
            }
        }
        
        /// <summary>
        /// Busca uma entidade do tipo CheckList pelo ID.
        /// </summary>
        /// <param name="id">ID do checklist a ser buscado.</param>
        /// <returns>Retorna o DTO de resposta contendo as informações do checklist encontrado.</returns>
        /// <exception cref="ArgumentException">Lançada se o ID for menor ou igual a zero.</exception>
        /// <exception cref="KeyNotFoundException">Lançada se o checklist com o ID especificado não for encontrado.</exception>
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
        
        /// <summary>
        /// Busca todas as entidades do tipo CheckList armazenadas no banco de dados.
        /// </summary>
        /// <returns>Retorna uma lista de DTOs de resposta contendo as informações de todos os checklists.</returns>
        public async Task<List<CheckListResponseDTO>> BuscarTodosAsync()
        {

            var checks = await _repositoryCheckList.BuscarTodos();

            List<CheckListResponseDTO> checklists = checks
                .Select(i => _mapper.Map<CheckListResponseDTO>(i))
                .ToList();

            return checklists;
        }

        /// <summary>
        /// Atualiza parcialmente uma entidade do tipo CheckList com base no DTO fornecido.
        /// </summary>
        /// <param name="checkListPatchDto">Objeto contendo os dados a serem atualizados do checklist.</param>
        /// <returns>Retorna true se a atualização for bem-sucedida.</returns>
        /// <exception cref="KeyNotFoundException">Lançada se o checklist com o ID especificado não for encontrado.</exception>
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

        /// <summary>
        /// Remove uma entidade do tipo CheckList pelo ID.
        /// </summary>
        /// <param name="id">ID do checklist a ser removido.</param>
        /// <returns>Retorna true se o checklist for removido com sucesso.</returns>
        /// <exception cref="ArgumentException">Lançada se o ID for menor ou igual a zero.</exception>
        /// <exception cref="KeyNotFoundException">Lançada se o checklist com o ID especificado não for encontrado.</exception>
        public async Task<bool> ApagarAsync(int id)
        {
            if(id <= 0)
                throw new ArgumentException("O ID deve ser maior que zero.", nameof(id));
            
            var result = await _repositoryCheckList.Apagar(id);

            if (!result)
                throw new KeyNotFoundException("CheckList não encontrado");

            return true;
        }
        
        /// <summary>
        /// Remove uma relação entre Cliente e CheckList com base nos IDs fornecidos.
        /// </summary>
        /// <param name="clienteId">ID do cliente associado.</param>
        /// <param name="checkListId">ID do checklist associado.</param>
        /// <returns>Retorna true se a relação for removida com sucesso.</returns>
        /// <exception cref="ApplicationException">Lançada se a operação não for realizada com sucesso.</exception>
        public async Task<bool> ApagarClienteRespondeCheckAsync(int clienteId, int checkListId)
        {
            
            var resultado = await _repositoryClienteRespondeCheck.Apagar(clienteId, checkListId);
            
            if(!resultado)
                throw new ApplicationException("Operação não realizada");
            
            return resultado;
            
        }
        
        /// <summary>
        /// Remove todas as entidades do tipo CheckList armazenadas no banco de dados.
        /// </summary>
        /// <returns>Retorna true se todas as entidades forem removidas com sucesso.</returns>
        /// <exception cref="ApplicationException">Lançada se a operação não for realizada com sucesso.</exception>
        public async Task<bool> ApagarTodosAsync()
        {
            var result = await _repositoryCheckList.ApagarTodos();

            if (!result)
                throw new ApplicationException("Operação não foi realizada");

            return true;

        }
        
        /// <summary>
        /// Refatora o modelo ClienteRespondeCheckList para manter apenas as informações necessárias.
        /// </summary>
        /// <param name="clienteRespondeCheckListModel">Modelo ClienteRespondeCheckList a ser refatorado.</param>
        /// <returns>Retorna o modelo refatorado de ClienteRespondeCheckList.</returns>
        /// <exception cref="ArgumentNullException">Lançada se o modelo ClienteRespondeCheckList for nulo.</exception>
        public ClienteRespondeCheckListModel? RefatoraoMinClienteRespondeCheckList(
            
            ClienteRespondeCheckListModel? clienteRespondeCheckListModel
            
            )
        {
            
            if(clienteRespondeCheckListModel is null)
                throw new ArgumentNullException(nameof(clienteRespondeCheckListModel));

            if (clienteRespondeCheckListModel.Cliente != null)
                clienteRespondeCheckListModel.Cliente = new ClienteModel
                {
                    Id = clienteRespondeCheckListModel.Cliente.Id,
                    Nome = clienteRespondeCheckListModel.Cliente.Nome,
                    Telefone = clienteRespondeCheckListModel.Cliente.Telefone,
                    Foto = clienteRespondeCheckListModel.Cliente.Foto
                    // Não incluímos as outras relações, como Interacoes ou CheckLists
                };

            if (clienteRespondeCheckListModel.CheckList != null)
                clienteRespondeCheckListModel.CheckList = new CheckListModel
                {
                    Id = clienteRespondeCheckListModel.CheckList.Id,
                    Nome = clienteRespondeCheckListModel.CheckList.Nome,
                    DataCriacao = clienteRespondeCheckListModel.CheckList.DataCriacao
                    // Não incluímos as outras relações, como Questoes ou Clientes
                };
            return clienteRespondeCheckListModel;
        }

        /// <summary>
        /// Refatora o modelo CheckList para manter apenas as informações necessárias.
        /// </summary>
        /// <param name="checkListModel">Modelo CheckList a ser refatorado.</param>
        /// <returns>Retorna o modelo refatorado de CheckList.</returns>
        /// <exception cref="ArgumentNullException">Lançada se o modelo CheckList for nulo.</exception>
        public CheckListModel RefatoraoMinCheckListModel(CheckListModel checkListModel)
        {
            if (checkListModel == null)
            {
                throw new ArgumentNullException(nameof(checkListModel));
            }

            checkListModel.Usuario = checkListModel.Usuario != null
                ? new UsuarioModel
                {
                    Id = checkListModel.Usuario.Id,
                    Nome = checkListModel.Usuario.Nome,
                    Telefone = checkListModel.Usuario.Telefone
                }
                : null;

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
        
        /// <summary>
        /// Gera um relatório geral para um checklist específico.
        /// </summary>
        /// <param name="idChecklist">ID do checklist para o qual o relatório deve ser gerado.</param>
        /// <returns>Retorna uma lista de DTOs contendo os dados do relatório gerado.</returns>
        public async Task<List<CheckListRelatorioGeralDTO>> GerarRelatorioGeralAsync(int idChecklist)
        {
            var relatorioDinamico = await _repositoryCheckList.GerarRelatorioGeral(idChecklist);

            // Mapear cada item dinâmico para o DTO
            var relatorioDto = relatorioDinamico.Select(item => new CheckListRelatorioGeralDTO
            {
                Id_interacao = item.id_interacao,
                Nome_cliente = item.nome_cliente,
                Nome_checklist = item.nome_checklist,
                Data_interacao = item.data_interacao,
                questao = item.questao,
                Id_resposta = item.id_resposta,
                alternativa = item.alternativa
            }).ToList();

            return relatorioDto;
        }
        
    }
}
