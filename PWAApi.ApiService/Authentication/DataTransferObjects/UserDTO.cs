using System.ComponentModel.DataAnnotations;

namespace PWAApi.ApiService.Authentication.DataTransferObjects
{   
    public class LoginDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class AdminUserDTO: LoginDTO
    {
        public string Name { get; set; } = string.Empty;
    }

    public class RegisterDTO : LoginDTO
    {
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
