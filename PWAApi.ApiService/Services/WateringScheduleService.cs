using OpenAI.Chat;
using PWAApi.ApiService.Helpers;
using PWAApi.ApiService.Models;
using PWAApi.ApiService.Services.AI;

namespace PWAApi.ApiService.Services
{
    public class WateringScheduleService
    {

        protected readonly IAIService _aiService;

        public WateringScheduleService(IAIService aiService)
        {
            _aiService = aiService;
        }

        public async Task<IndoorWateringSchedule?> GetSuggestedIndoorWateringSchedule(string species)
        {
            if (string.IsNullOrEmpty(species))
            {
                throw new Exception("No species name provided.");
            }

            ChatCompletionOptions options = OpenAIHelper.SetChatCompletionOptions<IndoorWateringSchedule>("suggested_watering_schedule");
            var userMessages = OpenAIHelper.SetUserChatMessages(new List<string>() {
                $"Suggest ideal conditions for maintaining a healthy indoor plant of species: {species}",
                "Avoid creative variations",
                "Always use the same values for the same species"
            });
            var result = await _aiService.Ask<IndoorWateringSchedule>(options, userMessages.Cast<ChatMessage>().ToList());

            return result;
        }
    }
}
