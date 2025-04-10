namespace PWAApi.ApiService.Services
{
    public interface IAIService
    {
        public Task<string> AskAI(string question);
    }
}
