using PWAApi.ApiService.DataTransferObjects;
using PWAApi.ApiService.Repositories;

namespace PWAApi.ApiService.Services.PlantID
{
    public class PlantIDServiceBase
    {
        protected readonly TaxonomyRepository _taxonomyRepository;
        protected readonly WikimediaService _wikimediaService;

        public PlantIDServiceBase(
            TaxonomyRepository taxonomyRepository,
            WikimediaService wikiMediaService)
        {
            _taxonomyRepository = taxonomyRepository;
            _wikimediaService = wikiMediaService;
        }

        public async Task<IEnumerable<PlantIDSearchResultDTO>?> IdentifyPlantAsync(string searchTerm)
        {
            List<PlantIDSearchResultDTO> results = new List<PlantIDSearchResultDTO>();

            try
            {
                var searchResults = await _taxonomyRepository.Search(searchTerm);

                foreach (var searchResult in searchResults)
                {
                    var imageDTO = await _wikimediaService.GetImageFromWikimediaAsync(searchResult.ScientificName);
                    if (imageDTO != null)
                    {
                        PlantIDSearchResultDTO dto = new PlantIDSearchResultDTO
                        {
                            ScientificName = searchResult.ScientificName,
                            CommonName = string.Join(", ", searchResult.VernacularNames.Select(x => x.Name)),
                            Images = imageDTO.ToList()
                        };

                        results.Add(dto);
                    }
                }

                return results;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error encountered while searching for: {searchTerm}. Error: {ex.Message}");
            }
        }
    }
}
