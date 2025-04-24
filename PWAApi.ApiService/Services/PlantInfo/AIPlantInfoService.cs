using System.Collections;
using System.Text.Json;
using AutoMapper;
using OpenAI.Chat;
using PWAApi.ApiService.DataTransferObjects.PlantID;
using PWAApi.ApiService.Helpers;
using PWAApi.ApiService.Models.PlantID;
using PWAApi.ApiService.Services.AI;

namespace PWAApi.ApiService.Services.PlantInfo
{
    public class AIPlantInfoService : IPlantInfoService
    {
        protected readonly IAIService _aiService;
        protected readonly WikimediaService _wikimediaService;

        public AIPlantInfoService(IMapper mapper, IAIService aiService, WikimediaService wikimediaService)
        {
            _aiService = aiService;
            _wikimediaService = wikimediaService;
        }

        public async Task<PlantDTO?> GetPlantAsync(string species)
        {
            var aiTask = GetPlantAI(species);
            var imageTask = _wikimediaService.GetImageFromWikimediaAsync(species);

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
                "Provide a consistent and factual description of the flower.",
                "Avoid creative variations. Always use the same description for the same flower.",
                "Include information about edibility and toxicity for humans, dogs, and cats broken down by part of plant.",
            });
            var result = await _aiService.Ask<PlantDTO>(options, chatMessages.Cast<ChatMessage>().ToList());

            return result;
        }
    }
}
