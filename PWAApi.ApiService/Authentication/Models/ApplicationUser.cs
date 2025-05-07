using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PWAApi.ApiService.Authentication.Models
{
    public class ApplicationUser : IdentityUser
    {
        //The Id in the auth provider system.
        //This will be empty for Adjutum users.
        public string? ProviderId { get; set; }

        //This will be 'Google' for now, but could be something like Facebook in the future. Will be null for Adjutum users.
        public string? Provider { get; set; } 
        
        [Required]
        public string Name { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
