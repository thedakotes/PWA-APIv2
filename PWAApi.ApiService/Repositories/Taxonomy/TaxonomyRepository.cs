using EventApi.Data;
using Microsoft.EntityFrameworkCore;
using PWAApi.ApiService.Models.Taxonomy;

namespace PWAApi.ApiService.Repositories
{
    public class TaxonomyRepository : Repository<TaxonomyRepository>
    {
        public TaxonomyRepository(AppDbContext appDbContext) : base(appDbContext) { }

        public async Task<IEnumerable<Taxonomy>> Search(string searchTerm)
        {
            return await _context.Taxonomy
                .Include(x => x.VernacularNames)
                .Where(x => 
                    x.ScientificName.Contains(searchTerm) || 
                    x.AcceptedScientificName.Contains(searchTerm) ||
                    x.Species.Contains(searchTerm) ||
                    x.Genus.Contains(searchTerm) ||
                    x.Family.Contains(searchTerm) ||
                    x.VernacularNames.Any(vernacularName => EF.Functions.Like(vernacularName.Name, $"%{searchTerm}%"))
                )
                .OrderBy(x => x.ScientificName)
                .ThenBy(x => x.Genus)
                .ThenBy(x => x.Family)
                .ToListAsync();
        }
    }
}
