using System.Text.Json;
using OpenAI.Chat;
using PWAApi.ApiService.DataTransferObjects.PlantID;

namespace PWAApi.ApiService.Services
{
    public class OpenAIService : IAIService
    {
        private readonly string apiKey = "";
        public OpenAIService(IConfiguration configuration)
        {
            //To get an API key, create an account here: https://auth.openai.com/
            //and then store it in your User Secrets for the project
            apiKey = configuration["OpenAI:ApiKey"] ?? throw new ArgumentNullException("OpenAI:ApiKey missing");
        }

        public async Task<string> AskAI(string question)
        {
            var schema = """
                        {
                            "type": "object",
                            "properties": {
                                "flowers": { 
                                    "type": "array",
                                    "items": {
                                        "type": "object",
                                        "properties": {
                                            "name": { "type": "string" },
                                            "color": { "type": "string" }
                                        },
                                        "required": ["name", "color"],
                                        "additionalProperties": false
                                    }
                                }
                            },
                            "required": ["flowers"],
                            "additionalProperties": false
                        }
                        """;
            ChatCompletionOptions options = new()
            {
                ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                    jsonSchemaFormatName: "suggest_flowers",
                    jsonSchema: BinaryData.FromBytes((System.Text.Encoding.UTF8.GetBytes(schema)).ToArray()),
                    jsonSchemaIsStrict: true)
            };
            var chat = new ChatClient(model: "gpt-4o-mini", apiKey: apiKey);
            //var test = await chat.CompleteChatAsync(question);
            var completion = (await chat.CompleteChatAsync(new[]
            {
                new UserChatMessage(question)
            }, options)).Value;
            using JsonDocument structuredJson = JsonDocument.Parse(completion.Content[0].Text);
            return completion.Content[0].Text;
        }

        public async Task<T?> Ask<T>(ChatCompletionOptions options, UserChatMessage[] userChatMessages)
        {
            var chat = new ChatClient(model: "gpt-4o-mini", apiKey: apiKey);
            var completion = (await chat.CompleteChatAsync(userChatMessages, options)).Value;
            using (JsonDocument structuredJson = JsonDocument.Parse(completion.Content[0].Text))
            {
                string jsonString = structuredJson.RootElement.GetRawText();
                try
                {
                    var deserializedObject = JsonSerializer.Deserialize<T>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return deserializedObject;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
