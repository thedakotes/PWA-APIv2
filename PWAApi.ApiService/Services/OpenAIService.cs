using System.Text.Json;
using OpenAI.Chat;

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
            ChatCompletionOptions options = new()
            {
                ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                    jsonSchemaFormatName: "suggest_flowers",
                    jsonSchema: BinaryData.FromBytes("""
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
                        """u8.ToArray()),
                    jsonSchemaIsStrict: true)
            };
            var chat = new ChatClient(model: "gpt-4o-mini", apiKey: apiKey);
            //var test = await chat.CompleteChatAsync(question);
            var completion = (await chat.CompleteChatAsync(new[]
            {
                new UserChatMessage(question)
            }, options)).Value;
            using JsonDocument structuredJson = JsonDocument.Parse(completion.Content[0].Text);
            Console.WriteLine(structuredJson.RootElement.GetProperty("interval"));
            return completion.Content[0].Text;
        }
    }
}
