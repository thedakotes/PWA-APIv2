using System.Collections;
using System.Text.Json;
using AutoMapper;
using OpenAI.Chat;
using PWAApi.ApiService.DataTransferObjects.PlantID;
using PWAApi.ApiService.Models.PlantID;
using PWAApi.ApiService.Services.AI;

namespace PWAApi.ApiService.Services.PlantInfo
{
    public class AIPlantInfoService : IPlantInfoService
    {
        protected readonly IAIService _aiService;

        public AIPlantInfoService(IMapper mapper, IAIService aiService)
        {
            _aiService = aiService;
        }

        public async Task<PlantDTO?> GetPlantAsync(string species)
        {
            var aiTask = GetPlantAI(species);
            var imageTask = GetPlantImageFromWikimediaAsync(species);

            await Task.WhenAll(aiTask, imageTask);

            var aiResult = await aiTask;
            var imageResult = await imageTask;

            if (aiResult != null && imageResult != null)
            {
                aiResult.Image = imageResult;
            }

            return aiResult;
        }

        protected async Task<PlantDTO?> GetPlantAI(string species)
        {
            var schema = JsonSchemaGenerator.GenerateJsonSchema<PlantDTO>();
            ChatCompletionOptions options = new()
            {
                Temperature = (float?)0,
                ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                    jsonSchemaFormatName: "species_information",
                    jsonSchema: BinaryData.FromBytes(System.Text.Encoding.UTF8.GetBytes(schema)),
                    jsonSchemaIsStrict: true)
            };
            var userMessages = new[]
            {
                new UserChatMessage($"Give us information for the plant species: '{species}'. Provide a consistent and factual description of the flower. Avoid creative variations. Always use the same description for the same flower. Include information about edibility and toxicity for humans, dogs, and cats broken down by part of plant. Include a freely available image url for reference.")
            };
            var result = await _aiService.Ask<PlantDTO>(options, userMessages);

            return result;
        }

        public async Task<ImageDTO?> GetPlantImageFromWikimediaAsync(string plantName)
        {
            using var httpClient = new HttpClient();

            var requestUrl = $"https://commons.wikimedia.org/w/api.php" +
                             $"?action=query&format=json&generator=search" +
                             $"&gsrsearch={Uri.EscapeDataString(plantName)}" +
                             $"&gsrlimit=1&gsrnamespace=6" + // Only search file pages
                             $"&prop=imageinfo&iiprop=url|extmetadata" +
                             $"&origin=*";


            var response = await httpClient.GetAsync(requestUrl);
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("query", out var queryElement) ||
                !queryElement.TryGetProperty("pages", out var pagesElement))
            {
                return null;
            }

            foreach (var page in pagesElement.EnumerateObject())
            {
                var imageInfoArray = page.Value.GetProperty("imageinfo");

                if (imageInfoArray.GetArrayLength() == 0) continue;

                var imageInfo = imageInfoArray[0];

                var url = imageInfo.GetProperty("url").GetString() ?? string.Empty;

                // Extract license data from extmetadata if available
                var extMetadata = imageInfo.GetProperty("extmetadata");

                string license = extMetadata.GetProperty("LicenseShortName").GetProperty("value").GetString() ?? "Unknown";
                string licenseUrl = extMetadata.GetProperty("LicenseUrl").GetProperty("value").GetString() ?? string.Empty;
                string attribution = extMetadata.GetProperty("Artist").GetProperty("value").GetString() ?? string.Empty;

                return new ImageDTO(url, license, licenseUrl, attribution);
            }

            return null;
        }

    }
}
