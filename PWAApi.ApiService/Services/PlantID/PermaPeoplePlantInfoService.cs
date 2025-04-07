using API.Services.PlantID;
using AutoMapper;
using PWAApi.ApiService.DataTransferObjects.PlantID;
using PWAApi.ApiService.Models.PlantID.PermaPeople;

namespace PWAApi.ApiService.Services.PlantID
{
    public class PermaPeoplePlantInfoService : PlantInfoServiceBase, IPlantInfoService
    {
        protected readonly string _keyID;
        public PermaPeoplePlantInfoService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IMapper mapper) : base(httpClientFactory, configuration, mapper, "PermaPeople:ApiKey", "https://permapeople.org/api/") 
        {
            _keyID = configuration["PermaPeople:KeyID"] ?? throw new Exception("Perma People Key ID is missing");
        }

        public async Task<IEnumerable<PlantDTO>> GetPlantSpeciesAsync(string species)
        {
            string queryParams = $"q={species}";
            string requestUrl = $"{_url}search?{queryParams}";
            var results = await FetchPlantSpeciesAsync<ResponseSchema>(requestUrl, HttpMethod.Post, ("x-permapeople-key-id", _keyID), ("x-permapeople-key-secret", _apiKey));
            return results.Plants != null ? results.Plants.Select(x => _mapper.Map<PlantDTO>(x)) : new List<PlantDTO>();
        }
    }
}
