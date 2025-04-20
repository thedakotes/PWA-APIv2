using EventApi.Data;
using Microsoft.EntityFrameworkCore;
using PWAApi.ApiService.Models.Taxonomy;

namespace PWAApi.ApiService.Helpers.Seeders
{
    public class VernacularNameSeeder : ISeeder
    {
        private CSVSeeder<VernacularName> _csvSeeder;
        private IEnumerable<int> validKeys = new List<int>();

        public VernacularNameSeeder(AppDbContext appDbContext)
        {
            _csvSeeder = new CSVSeeder<VernacularName>(
                "Data/SeedData/vernacularname.csv", 
                vernacularName => vernacularName.Name,
                vernacularName => vernacularName.TaxonKey.HasValue && validKeys.Contains(vernacularName.TaxonKey.Value));
        }

        public async Task SeedAsync(AppDbContext appDbContext)
        {
            await new TaxonomySeeder().SeedAsync(appDbContext);

            validKeys = appDbContext.Taxonomy
                .AsNoTracking()
                .Select(x => x.TaxonKey)
                .ToHashSet();

            await _csvSeeder.SeedAsync(appDbContext);
        }
    }
}
