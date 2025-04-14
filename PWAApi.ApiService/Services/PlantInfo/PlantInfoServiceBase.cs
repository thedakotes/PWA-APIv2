using System.Collections;
using System.Text.Json;
using AutoMapper;

namespace PWAApi.ApiService.Services.PlantInfo
{
    public class PlantInfoServiceBase
    {
        protected string _url;
        protected readonly string _apiKey;
        protected readonly HttpClient _httpClient;
        protected readonly IMapper _mapper;

        public PlantInfoServiceBase(IHttpClientFactory httpClientFactory, IConfiguration config, IMapper mapper, string apiKeyName, string url)
        {
            _httpClient = httpClientFactory.CreateClient();
            _apiKey = config[apiKeyName] ?? throw new Exception($"{apiKeyName} is missing");
            _mapper = mapper;
            _url = url;
        }

        protected async Task<T> FetchPlantSpeciesAsync<T>(string requestUrl, HttpMethod httpMethod, params (string key, string value)[] headers)
        {
            var request = new HttpRequestMessage(httpMethod, requestUrl);
            ArrayList results = new ArrayList();
            results.Add(_apiKey);
            results.Add(_httpClient);

            foreach (var header in headers)
            {
                request.Headers.Add(header.key, header.value);
            }

            var response = await _httpClient.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();
            try
            {
                var result = JsonSerializer.Deserialize<T>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result == null ? throw new Exception("Failed to parse response") : result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
