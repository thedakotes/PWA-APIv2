using System.Text.Json;
using API.Services.PlantID;
using AutoMapper;
using OpenAI.Chat;
using PWAApi.ApiService.DataTransferObjects.PlantID;
using PWAApi.ApiService.Models.PlantID.PermaPeople;

namespace PWAApi.ApiService.Services.PlantID
{
    public class PermaPeoplePlantInfoService : PlantInfoServiceBase, IPlantInfoService
    {
        protected readonly string _keyID;
        protected readonly IAIService _aiService;

        public PermaPeoplePlantInfoService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IMapper mapper,
            IAIService aiService) : base(httpClientFactory, configuration, mapper, "PermaPeople:ApiKey", "https://permapeople.org/api/")
        {
            _aiService = aiService;
            _keyID = configuration["PermaPeople:KeyID"] ?? throw new Exception("Perma People Key ID is missing");
        }

        public async Task<IEnumerable<PlantDTO>> GetPlantSpeciesAsync(string species)
        {
            if (string.IsNullOrEmpty(species))
            {
                throw new ArgumentException("Species name cannot be null or empty", nameof(species));
            }
            var results = await GetResponse(species);
            return results.Plants != null ? results.Plants.Select(x => _mapper.Map<PlantDTO>(x)) : new List<PlantDTO>();
        }

        public async Task<PlantDTO?> GetPlantAsync(string species)
        {
            var schema = JsonSchemaGenerator.GenerateJsonSchema<PlantDTO>();
            ChatCompletionOptions options = new()
            {
                ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                    jsonSchemaFormatName: "species_information",
                    jsonSchema: BinaryData.FromBytes(System.Text.Encoding.UTF8.GetBytes(schema)),
                    jsonSchemaIsStrict: true)
                };
            var userMessages = new[]
            {
                new UserChatMessage($"Give us information for the plant species: '{species}'. Additionally give us the 3 closest species based on native location.")
            };
            var result = await _aiService.Ask<PlantDTO>(options, userMessages);
            return result;
        }

        private async Task<ResponseSchema> GetResponse(string species)
        {
            string queryParams = $"q={species}";
            string requestUrl = $"{_url}search?{queryParams}";
            var results = await FetchPlantSpeciesAsync<ResponseSchema>(requestUrl, HttpMethod.Post, ("x-permapeople-key-id", _keyID), ("x-permapeople-key-secret", _apiKey));
            return results;
        }
    }
}
