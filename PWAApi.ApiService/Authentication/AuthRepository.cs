using EventApi.Data;
using Microsoft.EntityFrameworkCore;
using PWAApi.ApiService.Authentication.Models;

namespace PWAApi.ApiService.Authentication
{
    public class AuthRepository : Repository<ApplicationUser>, IAuthRepository
    {
        
        public AuthRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<ApplicationUser?> GetUserByProviderID(string providerID)
        {
            return await _dbSet.Where(p => p.ProviderId == providerID).FirstOrDefaultAsync();
        }

        public async Task<ApplicationUser?> GetUserByEmail(string email)
        {
            return await _dbSet.Where(p => p.Email == email).FirstOrDefaultAsync();
        }
    }
}
