namespace PWAApi.ApiService.Services
{
    public interface IEntityService<T, DTO, CreateDTO> where T : class
    {
        public Task<DTO> Get(int id);

        public Task<IEnumerable<DTO>> GetAll();

        public Task<DTO> Add(CreateDTO dataTransferObject);

        public Task<DTO> Update(DTO dataTransferObject);

        public Task Delete(int id);
    }
}
