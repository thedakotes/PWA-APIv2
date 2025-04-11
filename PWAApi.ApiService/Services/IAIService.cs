using System.Text.Json;
using OpenAI.Chat;

namespace PWAApi.ApiService.Services
{
    public interface IAIService
    {
        public Task<string> AskAI(string question);

        public Task<T?> Ask<T>(ChatCompletionOptions options, UserChatMessage[] userChatMessages);
    }
}
