using EventApi.Data;

namespace PWAApi.ApiService.Helpers.Seeders
{
    public interface ISeeder
    {
        Task SeedAsync(AppDbContext dbContext);
    }
}
