namespace PWAApi.ApiService.Middleware
{
    public class ConfigurationException : Exception
    {
        public ConfigurationException(string message) : base(message) { }
    }
}
