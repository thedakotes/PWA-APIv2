using PWAApi.ApiService.Authentication.Models;

namespace PWAApi.ApiService.Authentication
{
    public interface IAuthRepository : IRepository<ApplicationUser>
    {
        Task<ApplicationUser?> GetUserByProviderID(string providerID);
        Task<ApplicationUser?> GetUserByEmail(string email);
    }
}
