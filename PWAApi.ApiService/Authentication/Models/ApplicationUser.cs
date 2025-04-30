using System.ComponentModel.DataAnnotations;

namespace PWAApi.ApiService.Authentication.Models
{
    public class ApplicationUser : BaseEntity
    {
        [Required]
        public string ProviderId { get; set; } = string.Empty; //The Id in the auth provider system. This is Google right now, but maybe things like FB in the future?
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
