using OpenAI.Assistants;
using OpenAI.Chat;

namespace PWAApi.ApiService.Helpers
{
    public static class OpenAIHelper
    {
        public static ChatCompletionOptions SetChatCompletionOptions<T>(string jsonSchemaFormatName, double temperature = 0.0, bool jsonSchemaIsStrict = true, IEnumerable<ChatTool>? tools = null)
        {
            var schema = JsonSchemaGenerator.GenerateJsonSchema<T>();
            ChatCompletionOptions options = new()
            {
                Temperature = (float?)temperature,
                ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                    jsonSchemaFormatName: jsonSchemaFormatName,
                    jsonSchema: BinaryData.FromBytes(System.Text.Encoding.UTF8.GetBytes(schema).ToArray()),
                    jsonSchemaIsStrict: jsonSchemaIsStrict),
            };

            if (tools != null)
            {
                foreach (var tool in tools)
                {
                    options.Tools.Add(tool);
                }
            }

            return options;
        }

        public static List<SystemChatMessage> SetSystemChatMessages(IEnumerable<string> messages)
        {
            return messages.Select(x => new SystemChatMessage(x)).ToList();
        }

        public static List<UserChatMessage> SetUserChatMessages(IEnumerable<string> messages)
        {
            return messages.Select(x => new UserChatMessage(x)).ToList();
        }
    }
}
