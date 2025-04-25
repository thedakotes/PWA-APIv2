using System.Security.AccessControl;
using Microsoft.OpenApi.Services;
using PWAApi.ApiService.DataTransferObjects;
using PWAApi.ApiService.Models.Taxonomy;
using PWAApi.ApiService.Repositories;
using PWAApi.ApiService.Services.Caching;

namespace PWAApi.ApiService.Services.PlantID
{
    public class PlantIDServiceBase
    {
        protected readonly TaxonomyRepository _taxonomyRepository;
        protected readonly WikimediaService _wikimediaService;
        private const string PlantNetCacheKey = "plant-net";
        private readonly ICacheService _cacheService;


        public PlantIDServiceBase(
            ICacheService cacheService,
            TaxonomyRepository taxonomyRepository,
            WikimediaService wikiMediaService)
        {
            _taxonomyRepository = taxonomyRepository;
            _wikimediaService = wikiMediaService;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<PlantIDSearchResultDTO>?> IdentifyPlantAsync(string searchTerm)
        {
            try
            {
                var cacheKey = $"{PlantNetCacheKey}:{searchTerm}";
                var cached = await _cacheService.GetAsync<IEnumerable<PlantIDSearchResultDTO>>(cacheKey);
                if (cached != null && cached?.Count() != 0)
                    return cached;

                var searchResults = await _taxonomyRepository.Search(searchTerm);

                List<Task<PlantIDSearchResultDTO>> tasks = new List<Task<PlantIDSearchResultDTO>>();

                foreach (var searchResult in searchResults)
                {
                    tasks.Add(Set(searchResult));
                }

                List<PlantIDSearchResultDTO> results = (await Task.WhenAll(tasks)).ToList();
                await _cacheService.SetAsync(cacheKey, results, TimeSpan.FromMinutes(10));
                return results;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error encountered while searching for: {searchTerm}. Error: {ex.Message}");
            }
        }

        private async Task<PlantIDSearchResultDTO> Set(Taxonomy taxonomy)
        {
            PlantIDSearchResultDTO dto = new PlantIDSearchResultDTO(taxonomy.ScientificName, string.Join(", ", taxonomy.VernacularNames.Select(x => x.Name)));

            var imageResults = await _wikimediaService.GetImageFromWikimediaAsync(taxonomy.ScientificName);
            if (imageResults != null)
            {
                dto.Images = imageResults.ToList();

            }

            return dto;
        }
    }
}
