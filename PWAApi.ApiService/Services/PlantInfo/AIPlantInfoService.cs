using System.Collections;
using System.Text.Json;
using AutoMapper;
using OpenAI.Chat;
using PWAApi.ApiService.DataTransferObjects.PlantID;
using PWAApi.ApiService.Helpers;
using PWAApi.ApiService.Models.PlantID;
using PWAApi.ApiService.Services.AI;
using PWAApi.ApiService.Services.Caching;

namespace PWAApi.ApiService.Services.PlantInfo
{
    public class AIPlantInfoService : IPlantInfoService
    {
        protected readonly IAIService _aiService;
        protected readonly ICacheService _cacheService;
        protected readonly WikimediaService _wikimediaService;

        public AIPlantInfoService(IMapper mapper, IAIService aiService, ICacheService cacheService, WikimediaService wikimediaService)
        {
            _aiService = aiService;
            _cacheService = cacheService;
            _wikimediaService = wikimediaService;
        }

        public async Task<PlantDTO?> GetPlantAsync(string species)
        {
            var cacheKey = $"plant:info:{species}";
            var cachedResult = await _cacheService.GetAsync<PlantDTO>(cacheKey);

            if (cachedResult != null)
            {
                return cachedResult;
            }

            var result = await BuildPlantDTO(species);
            if (result != null)
            {
                await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromHours(1));
            }

            return result;
        }

        protected async Task<PlantDTO?> BuildPlantDTO(string species)
        {
            var aiTask = GetPlantAI(species);
            var imageTask = _wikimediaService.GetImagesFromWikimediaAsync(species);

            await Task.WhenAll(aiTask, imageTask);

            var aiResult = await aiTask;
            var imageResult = await imageTask;

            if (aiResult != null && imageResult != null)
            {
                aiResult.Images = imageResult;
            }

            return aiResult;
        }

        protected async Task<PlantDTO?> GetPlantAI(string species)
        {
            ChatCompletionOptions options = OpenAIHelper.SetChatCompletionOptions<PlantDTO>("species_information");
            var chatMessages = OpenAIHelper.SetSystemChatMessages(new List<string>() {
                $"Give us information for the plant species: '{species}'. ",
                "Provide a consistent and factual description of the plant species.",
                "Avoid creative variations. Always use the same description for the same plant.",
                "Include information about the plant's watering needs for every month of the year in milliliters. If the plant does not require water for that month then return 0 for the value.",
                "Include information about edibility and toxicity for humans, dogs, and cats broken down by part of plant.",
            });
            var result = await _aiService.Ask<PlantDTO>(options, chatMessages.Cast<ChatMessage>().ToList());

            return result;
        }
    }
}
