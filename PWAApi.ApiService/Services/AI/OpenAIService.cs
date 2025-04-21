using System.Text.Json;
using OpenAI.Chat;
using System.Text;
using OpenAI.Assistants;

namespace PWAApi.ApiService.Services.AI
{
    public class OpenAIService : IAIService
    {
        private readonly string apiKey = "";
        private readonly string model = "gpt-4o-mini";

        public OpenAIService(IConfiguration configuration)
        {
            //To get an API key, create an account here: https://auth.openai.com/
            //and then store it in your User Secrets for the project
            apiKey = configuration["OpenAI_ApiKey"] ?? throw new ArgumentNullException("OpenAI ApiKey missing");
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
                    jsonSchema: BinaryData.FromBytes(Encoding.UTF8.GetBytes(schema).ToArray()),
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

        public async Task<T?> Ask<T>(ChatCompletionOptions options, List<ChatMessage> chatMessages)
        {
            var chat = new ChatClient(model: model, apiKey: apiKey);
            ChatCompletion completion;
            try
            {
                completion = (await chat.CompleteChatAsync(chatMessages, options)).Value;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

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

        public async Task<T?> Ask<T>(ChatCompletionOptions options, List<ChatMessage> chatMessages, Dictionary<string, Func<ChatToolCall, Task<ToolChatMessage>>> _toolHandlers)
        {
            var chat = new ChatClient(model: model, apiKey: apiKey);
            ChatCompletion completion;
            try
            {
                bool requiresAction;

                do
                {
                    requiresAction = false;
                    completion = (await chat.CompleteChatAsync(chatMessages, options)).Value;

                    switch (completion.FinishReason)
                    {
                        case ChatFinishReason.Stop:
                            {
                                chatMessages.Add(new AssistantChatMessage(completion));
                                break;
                            }
                        case ChatFinishReason.ToolCalls:
                            {
                                chatMessages.Add(new AssistantChatMessage(completion));

                                foreach (ChatToolCall toolCall in completion.ToolCalls)
                                {
                                    if (_toolHandlers.TryGetValue(toolCall.FunctionName, out var handler))
                                    {
                                        ToolChatMessage toolResponse = await handler(toolCall);
                                        chatMessages.Add(toolResponse);
                                    }
                                    else
                                    {
                                        throw new NotImplementedException($"No handler defined for tool: {toolCall.FunctionName}");
                                    }
                                }

                                requiresAction = true;
                                break;
                            }
                        case ChatFinishReason.Length:
                            throw new NotImplementedException("Incomplete model output due to MaxTokens parameter or token limit exceeded.");

                        case ChatFinishReason.ContentFilter:
                            throw new NotImplementedException("Omitted content due to a content filter flag.");

                        case ChatFinishReason.FunctionCall:
                            throw new NotImplementedException("Deprecated in favor of tool calls.");

                        default:
                            throw new NotImplementedException(completion.FinishReason.ToString());
                    }
                } while (requiresAction);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

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
