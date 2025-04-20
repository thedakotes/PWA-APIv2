using System.Text.Json;
using AutoMapper;
using PWAApi.ApiService.DataTransferObjects;
using PWAApi.ApiService.DataTransferObjects.PlantID;
using PWAApi.ApiService.Models.PlantID.PlantNet;
using PWAApi.ApiService.Repositories;
using PWAApi.ApiService.Services;

namespace API.Services.PlantID
{
    public class PlantNetService : IPlantIDService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly TaxonomyRepository _gbifPlantRepository;
        private readonly WikimediaService _wikimediaService;
        private string url = "https://my-api.plantnet.org/v2/identify/all";

        public PlantNetService(IHttpClientFactory httpClientFactory, IConfiguration config, TaxonomyRepository gbifPlantRepository, IMapper mapper, WikimediaService wikimediaService)
        {
            _apiKey = config["PlantNet:ApiKey"] ?? throw new Exception("Plant Net API key is missing!");
            _httpClient = httpClientFactory.CreateClient();
            _gbifPlantRepository = gbifPlantRepository;
            _mapper = mapper;
            _wikimediaService = wikimediaService;
        }

        public async Task<IEnumerable<PlantIDDTO>> IdentifyPlantAsync(List<IFormFile> files)
        {
            var content = new MultipartFormDataContent();
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    SetContent(file, content);
                }
            }

            try
            {
                var result = await Post(content);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<PlantIDSearchResultDTO>?> IdentifyPlantAsync(string searchTerm)
        {
            List<PlantIDSearchResultDTO> results = new List<PlantIDSearchResultDTO>();

            try
            {
                var searchResults = await _gbifPlantRepository.Search(searchTerm);

                foreach (var searchResult in searchResults)
                {
                    var imageDTO = await _wikimediaService.GetImageFromWikimediaAsync(searchResult.ScientificName);
                    if (imageDTO != null)
                    {
                        PlantIDSearchResultDTO dto = new PlantIDSearchResultDTO
                        { 
                            ScientificName = searchResult.ScientificName, 
                            CommonName = "", 
                            Images = imageDTO.ToList() 
                        };

                        results.Add(dto);
                    }
                }

                return results;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error encountered while searching for: {searchTerm}. Error: {ex.Message}");
            }
        }

        private async Task<IEnumerable<PlantIDDTO>> Post(MultipartFormDataContent content)
        {
            string queryParams = $"include-related-images=true&no-reject=false&lang=en&api-key={_apiKey}";
            try
            {
                var response = await _httpClient.PostAsync($"{url}?{queryParams}", content);
                var responseBody = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<PlantNetIdentificationSchema>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (result?.Results != null)
                {
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

        private void SetContent(IFormFile file, MultipartFormDataContent content)
        {
            var streamContent = new StreamContent(file.OpenReadStream());
            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

            content.Add(streamContent, "images", file.FileName);
            content.Add(new StringContent("flower"), "organs");
        }

        private async Task<ImageDTO?> GetImageDTO(string searchTerm)
        {
            var imageDTOs = await _wikimediaService.GetImageFromWikimediaAsync(searchTerm);
            return imageDTOs?.FirstOrDefault();
        }
    }
}
