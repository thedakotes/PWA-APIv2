using PWAApi.ApiService.DataTransferObjects;
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
            List<PlantIDSearchResultDTO> results = new List<PlantIDSearchResultDTO>();

            try
            {
                var cacheKey = $"{PlantNetCacheKey}:{searchTerm}";
                var cached = await _cacheService.GetAsync<IEnumerable<PlantIDSearchResultDTO>>(cacheKey);
                if (cached != null && cached?.Count() != 0)
                    return cached;

                var searchResults = await _taxonomyRepository.Search(searchTerm);

                foreach (var searchResult in searchResults)
                {
                    var imageDTO = await _wikimediaService.GetImageFromWikimediaAsync(searchResult.ScientificName);
                    if (imageDTO != null)
                    {
                        PlantIDSearchResultDTO dto = new PlantIDSearchResultDTO
                        {
                            ScientificName = searchResult.ScientificName,
                            CommonName = "",
                            Images = imageDTO.ToList()
                        };

                        results.Add(dto);
                    }
                }

                await _cacheService.SetAsync(cacheKey, results, TimeSpan.FromMinutes(10));
                return results;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error encountered while searching for: {searchTerm}. Error: {ex.Message}");
            }
        }
    }
}
