using System.ComponentModel.DataAnnotations;

namespace PWAApi.ApiService.Authentication.Models
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string ProviderId { get; set; } = string.Empty;
        
        public string Email { get; set; } = string.Empty;
        
        public string Name { get; set; } = string.Empty;
    }
}
