namespace PWAApi.ApiService.Services
{
    public interface IEntityService<T, DTO> where T : class
    {
        public Task<DTO> Get(int id);

        public Task<IEnumerable<DTO>> GetAll();

        public Task<DTO> Add(DTO dataTransferObject);

        public Task Update(DTO dataTransferObject);

        public Task Delete(int id);
    }
}
