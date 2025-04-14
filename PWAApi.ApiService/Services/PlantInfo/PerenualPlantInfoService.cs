using AutoMapper;
using PWAApi.ApiService.DataTransferObjects.PlantID;
using PWAApi.ApiService.Models.PlantID.Perenual;

namespace PWAApi.ApiService.Services.PlantInfo
{
    public class PerenualPlantInfoService : PlantInfoServiceBase, IPlantInfoService
    {
        public PerenualPlantInfoService(IHttpClientFactory httpClientFactory, IConfiguration config, IMapper mapper) : base(httpClientFactory, config, mapper, "Perenual:ApiKey", "https://perenual.com/api/v2/")
        { }

        public Task<PlantDTO?> GetPlantAsync(string species)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PlantDTO>> GetPlantSpeciesAsync(string species)
        {
            string queryParams = $"key={_apiKey}&q={species}";
            string requestUrl = $"{_url}species-list?{queryParams}";
            var results = await FetchPlantSpeciesAsync<ResponseSchema>(requestUrl, HttpMethod.Post);
            return results.Data.Select(x => _mapper.Map<PlantDTO>(x));
        }
    }
}
