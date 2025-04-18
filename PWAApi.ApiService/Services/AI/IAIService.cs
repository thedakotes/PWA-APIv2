using System.Text.Json;
using OpenAI.Chat;

using API.DataTransferObjects;

namespace PWAApi.ApiService.Services.AI
{
    public interface IAIService
    {
        public Task<string> AskAI(string question);

        public Task<T?> Ask<T>(ChatCompletionOptions options, List<ChatMessage> chatMessages);

        public Task<T?> Ask<T>(ChatCompletionOptions options, List<ChatMessage> chatMessages, Dictionary<string, Func<ChatToolCall, Task<ToolChatMessage>>> _toolHandlers);
    }
}
