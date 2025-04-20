using EventApi.Data;
using PWAApi.ApiService.Models.Taxonomy;

namespace PWAApi.ApiService.Helpers.Seeders
{
    public class TaxonomySeeder : ISeeder
    {
        private CSVSeeder<Taxonomy> _csvSeeder;

        public TaxonomySeeder()
        {
            _csvSeeder = new CSVSeeder<Taxonomy>("Data/SeedData/iNaturalist_Plantae.csv", plant => plant.TaxonKey);
        }

        public async Task SeedAsync(AppDbContext appDbContext)
        {
            await _csvSeeder.SeedAsync(appDbContext);
        }
    }
}
