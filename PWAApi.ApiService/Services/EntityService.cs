using API.DataTransferObjects;
using API.Models;
using AutoMapper;

namespace PWAApi.ApiService.Services
{
    /// <summary>
    /// Base service class providing common CRUD operations for entities and their DTOs.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <typeparam name="DTO">The data transfer object type.</typeparam>
    /// <typeparam name="TRepository">The repository type implementing <see cref="IRepository{T}"/>.</typeparam>
    public abstract class EntityService<T, DTO, CreateDTO, TRepository> where T : class
        where TRepository : class, IRepository<T>
    {
        public readonly IMapper _mapper;
        public readonly TRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityService{T, DTO, TRepository}"/> class.
        /// </summary>
        /// <param name="mapper">The AutoMapper instance for mapping between entities and DTOs.</param>
        /// <param name="repository">The repository instance for data access.</param>
        protected EntityService(IMapper mapper, TRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        /// <summary>
        /// Retrieves a single DTO by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <returns>The mapped DTO if found; otherwise, the default value for <typeparamref name="DTO"/>.</returns>
        public async Task<DTO> Get(int id)
        {
            var model = await _repository.GetByIdAsync(id);
            var modelDTO = _mapper.Map<DTO>(model);
            return modelDTO;
        }

        /// <summary>
        /// Retrieves all entities and maps them to DTOs.
        /// </summary>
        /// <returns>An enumerable collection of mapped DTOs.</returns>
        public async Task<IEnumerable<DTO>> GetAll()
        {
            var models = await _repository.GetAllAsync();
            var modelDTOs = models.Select(x => _mapper.Map<DTO>(x));
            return modelDTOs;
        }

        /// <summary>
        /// Adds a new entity to the repository using the provided DTO.
        /// </summary>
        /// <param name="dataTransferObject">The DTO representing the entity to add.</param>
        /// <returns>The mapped DTO of the newly added entity.</returns>
        public async Task<DTO> Add(CreateDTO dataTransferObject)
        {
            var entity = _mapper.Map<T>(dataTransferObject);
            await _repository.AddAsync(entity);
            var modelDTO = _mapper.Map<DTO>(entity);
            return modelDTO;
        }

        /// <summary>
        /// Updates an existing entity in the repository using the provided DTO.
        /// </summary>
        /// <param name="dataTransferObject">The DTO representing the updated entity.</param>
        public async Task<DTO> Update(DTO dataTransferObject)
        {
            var entity = _mapper.Map<T>(dataTransferObject);
            await _repository.UpdateAsync(entity);
            var modelDTO = _mapper.Map<DTO>(entity);
            return modelDTO;
        }

        /// <summary>
        /// Deletes an entity from the repository by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to delete.</param>
        public async Task Delete(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
