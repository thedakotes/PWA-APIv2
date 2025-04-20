using EventApi.Data;
using PWAApi.ApiService.Helpers.Seeders;

namespace PWAApi.ApiService.Services.DbContext
{
    public class SeedManagerService
    {
        private readonly IEnumerable<ISeeder> _seeders;

        public SeedManagerService(IEnumerable<ISeeder> seeders)
        {
            _seeders = seeders;
        }

        public async Task RunAllAsync(AppDbContext dbContext)
        {
            foreach (var seeder in _seeders)
            {
                await seeder.SeedAsync(dbContext);
            }
        }
    }

}
