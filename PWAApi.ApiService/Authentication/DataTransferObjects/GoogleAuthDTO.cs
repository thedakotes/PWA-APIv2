namespace PWAApi.ApiService.Authentication.DataTransferObjects
{
    public class GoogleUserDTO
    {
        public string sub { get; set; } // Google's unique user ID
        public string email { get; set; }
        public string name { get; set; }
    }

    public class ExchangeTokenRequest
    {
        public string IdToken { get; set; }
    }
}
