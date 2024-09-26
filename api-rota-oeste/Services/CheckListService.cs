using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Services.Interfaces;
using AutoMapper;

namespace api_rota_oeste.Services
{
    public class CheckListService : ICheckListService
    {
        private readonly ICheckListRepository _repository;
        private readonly IMapper _mapper;

        public CheckListService(ICheckListRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CheckListResponseDTO> AddAsync(CheckListRequestDTO req)
        {
            CheckListModel check = await _repository.Add(req);

            return _mapper.Map<CheckListResponseDTO>(check);
        }

        public async Task<List<CheckListResponseDTO>> AddCollectionAsync(CheckListCollectionDTO req)
        {
            List<CheckListModel> checks = await _repository.AddCollection(req);

            if (checks == null || !checks.Any())
                throw new InvalidOperationException("Conteúdo não encontrado");

            List<CheckListResponseDTO> resp = checks
                .Select(i => _mapper.Map<CheckListResponseDTO>(i))
                .ToList();

            return resp;
        }

        public async Task<CheckListResponseDTO?> FindByIdAsync(int id)
        {
            var check = await _repository.FindById(id);

            if (check == null)
                throw new KeyNotFoundException("Entidade checklist não encontrada");

            return _mapper.Map<CheckListResponseDTO>(check);
        }

        public async Task<List<CheckListResponseDTO>> GetAllAsync()
        {

            List<CheckListModel?> checks = await _repository.GetAll();

            if (checks == null || !checks.Any())
                throw new InvalidOperationException("Conteúdo não encontrado");

            List<CheckListResponseDTO> resp = checks
                .Select(i => _mapper.Map<CheckListResponseDTO>(i))
                .ToList();

            return resp;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _repository.Delete(id);

            if (!result)
                throw new ApplicationException("Objeto não encontrado");

            return true;
        }
        public async Task<bool> DeleteAllAsync()
        {
            var result = await _repository.DeleteAll();

            if (!result)
                throw new ApplicationException("Operação não foi realizada");

            return true;

        }

    }
}
