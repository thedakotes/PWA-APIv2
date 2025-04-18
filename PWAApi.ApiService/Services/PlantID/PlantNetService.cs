using System.Text.Json;
using AutoMapper;
using AutoMapper.Internal;
using OpenAI.Chat;
using PWAApi.ApiService.DataTransferObjects;
using PWAApi.ApiService.DataTransferObjects.PlantID;
using PWAApi.ApiService.Helpers;
using PWAApi.ApiService.Models.PlantID;
using PWAApi.ApiService.Models.PlantID.PlantNet;
using PWAApi.ApiService.Services;
using PWAApi.ApiService.Services.AI;

namespace API.Services.PlantID
{
    public class PlantNetService : IPlantIDService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
        private readonly IAIService _aiService;
        private readonly IMapper _mapper;
        private readonly WikimediaService _wikimediaService;
        private string url = "https://my-api.plantnet.org/v2/identify/all";

        public PlantNetService(IHttpClientFactory httpClientFactory, IConfiguration config, IAIService aiService, IMapper mapper, WikimediaService wikimediaService)
        {
            _apiKey = config["PlantNet:ApiKey"] ?? throw new Exception("Plant Net API key is missing!");
            _httpClient = httpClientFactory.CreateClient();
            _aiService = aiService;
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
            ChatCompletionOptions options = OpenAIHelper.SetChatCompletionOptions<PlantIDSearchResultDTO[]>("plant_species_search", 1);
            var systemMessages = OpenAIHelper.SetSystemChatMessages(new List<string>() {
                "You are a botanical research assistant that specializes in identifying all plants that match or resemble a given common or scientific name.",
                $"Your task is to return an array of plant species, subspecies, or cultivars commonly associated with the search term: '{searchTerm}'.",
                "Be exhaustive. Consider all variations, synonyms, regional species, and cultivars that may resemble or be referred to by this name.",
                "Consider not just flowers but trees, crops, and other types of plants.",
                "You must return a list of at least 5 different species, but aim for as many as possible.",
                "Do not include variations on species that we are already returning.",
                "For example, for 'Tulip gesneriana' we only want a single result even t"
            });

            var userMessages = OpenAIHelper.SetUserChatMessages(new List<string>()
            {
                $"Provide an exhaustive list of plant species and subspecies related to the term: {searchTerm}.",
                "Base the comparison on scientific name and/or common names.",
                "For example, for 'Tulip', you might return Tulipa gesneriana, Tulipa sylvestris, Tulipa clusiana, etc.",
                "Do not limit to one result. Do not summarize."
            });
            IEnumerable<ChatMessage> allMessages = systemMessages.Cast<ChatMessage>().Concat(userMessages.Cast<ChatMessage>());

            try
            {
                var results = await _aiService.Ask<PlantIDResponse>(options, allMessages.ToList());

                if (results?.Items == null || !results.Items.Any())
                {
                    return null;
                }

                var fetchImageTasks = results.Items.Select(async item =>
                {
                    var imageDTO = await GetImageDTO(item.ScientificName);
                    return new PlantIDSearchResultDTO
                    {
                        ScientificName = item.ScientificName,
                        CommonName = item.CommonName,
                        Image = imageDTO,
                    };
                });

                var mappedResults = await Task.WhenAll(fetchImageTasks);
                return mappedResults.Where(x => x.Image != null);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to identify plant: {searchTerm}. Exception: {ex.Message}");
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
