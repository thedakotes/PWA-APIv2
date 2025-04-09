using OpenAI.Chat;

namespace PWAApi.ApiService.Services
{
    public class OpenAIService
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
            var chat = new ChatClient(model: "gpt-4o-mini", apiKey:apiKey);
            var test = await chat.CompleteChatAsync(question);
            return test.Value.Content[0].Text;
        }
    }
}
