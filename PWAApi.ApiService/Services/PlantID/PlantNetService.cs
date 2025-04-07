
using System.Text.Json;
using AutoMapper;
using PWAApi.ApiService.DataTransferObjects.PlantID;
using PWAApi.ApiService.Models.PlantID;

namespace API.Services.PlantID
{
    public class PlantNetService : IPlantIDService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private string url = "https://my-api.plantnet.org/v2/identify/all";

        public PlantNetService(IHttpClientFactory httpClientFactory, IConfiguration config, IMapper mapper)
        {
            _apiKey = config["PlantNet:ApiKey"] ?? throw new Exception("Plant Net API key is missing!");
            _httpClient = httpClientFactory.CreateClient();
            _mapper = mapper;
        }

        public async Task<List<PlantIDDTO>> IdentifyPlantAsync(List<IFormFile> files)
        {
            var content = new MultipartFormDataContent();
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var streamContent = new StreamContent(file.OpenReadStream());
                    streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

                    content.Add(streamContent, "images", file.FileName);
                    content.Add(new StringContent("flower"), "organs");
                }
            }

            string queryParams = $"include-related-images=true&no-reject=false&nb-results=10&lang=en&api-key={_apiKey}";
            try
            {
                var response = await _httpClient.PostAsync($"{url}?{queryParams}", content);
                var responseBody = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<PlantNetSchema>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
                if (result?.Results != null)
                {
                    Console.WriteLine(result.Results.FirstOrDefault()?.GetType().Name);
                    var mappedResults = result.Results.Select(x => _mapper.Map<PlantIDDTO>(x)).ToList();
                    return mappedResults;
                }
                else
                {
                    throw new Exception("Failed to parse PlantNet response");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
