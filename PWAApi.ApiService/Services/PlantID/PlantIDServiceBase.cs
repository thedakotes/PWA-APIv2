using System.Collections.Concurrent;
using PWAApi.ApiService.DataTransferObjects.PlantID;
using PWAApi.ApiService.Models;
using PWAApi.ApiService.Models.Taxonomy;
using PWAApi.ApiService.Repositories;
using PWAApi.ApiService.Services.Caching;

namespace PWAApi.ApiService.Services.PlantID
{
    /// <summary>
    /// Cache details
    /// 
    /// We store a PlantMetadataDTO object in the 'plant:search:[searchTerm]' key, which is a list of all
    /// the plants we got from the Taxonomy db when we searched for the term:
    /// 
    ///     PlantMetadataDTO (one for each taxon we found in our search)
    ///         ScientificName = item.ScientificName,
    ///         CommonName = "",
    ///         TaxonKey = item.TaxonKey
    /// 
    /// We store all the images (List<ImageDT0>) associated for each plant under the key: 'plant:image:[TaxonKey]'
    /// 
    /// If we get no images back for a plant, we don't save off that plant. I noticed we were excluding the plants with empty
    /// images on the front end anyway, so it seemed safe to exclude them from the cache as well as the results returned. If this is wrong, 
    /// we can change this. 
    /// 
    /// </summary>
    public class PlantIDServiceBase
    {
        protected readonly TaxonomyRepository _taxonomyRepository;
        protected readonly WikimediaService _wikimediaService;
        private const string PlantNetCacheKey = "plant";
        private readonly ICacheService _cacheService;

        //KEF: I'd like to extract out the cache TTL (time to live) to a setting on Azure. How are we handling this? Appsettings? This is a Monday problem.

        public PlantIDServiceBase(
            ICacheService cacheService,
            TaxonomyRepository taxonomyRepository,
            WikimediaService wikiMediaService)
        {
            _taxonomyRepository = taxonomyRepository;
            _wikimediaService = wikiMediaService;
            _cacheService = cacheService;
        }

        public async Task<PaginatedResult<PlantIDSearchResultDTO>?> IdentifyPlantAsync(string searchTerm, int page, int pageSize)
        {
            Console.WriteLine($"[PlantID] Search for '{searchTerm}' started.");

            var metadataCacheKey = $"{PlantNetCacheKey}:search:{searchTerm}:page{page}";
            var metadata = await _cacheService.GetAsync<IEnumerable<PlantMetadataDTO>>(metadataCacheKey);

            //If we found any cached data for this search term, grab it along with all of the images
            if (metadata != null && metadata.Any())
            {
                //Refresh our searchTerm cache key TTL. We could add an option to do this optionally in the future
                await _cacheService.SetAsync(metadataCacheKey, metadata, TimeSpan.FromHours(1));

                Console.WriteLine($"[PlantID] Found {metadata.Count()} cached records for '{searchTerm}'.");
                return await BuildFullResultsFromMetadataAsync(metadata);
            }

            return await FetchAndCacheSearchResultsAsync(searchTerm, metadataCacheKey, page, pageSize);

        }

        private async Task<PaginatedResult<PlantIDSearchResultDTO>> BuildFullResultsFromMetadataAsync(IEnumerable<PlantMetadataDTO> metadata)
        {
            var results = new List<PlantIDSearchResultDTO>();

            foreach (var meta in metadata)
            {
                var imageKey = $"{PlantNetCacheKey}:image:{meta.TaxonKey}";
                var images = await _cacheService.GetAsync<List<ImageDTO>>(imageKey)
                             //This extra part will cause problems if we want to change the caching strategy to also include plants that have no
                             //images, because every time we pull from the cache, we'll also do this call for all our missing images. 
                             //I have this here in case something went bananas, but it's possible we can remove this call entirely
                             ?? await _wikimediaService.GetImagesFromWikimediaAsync(meta.ScientificName);

                if (images != null)
                {
                    //We refresh our TTL for our images.
                    await _cacheService.SetAsync(imageKey, images, TimeSpan.FromHours(1));

                    results.Add(new PlantIDSearchResultDTO
                    {
                        ScientificName = meta.ScientificName,
                        CommonNames = meta.CommonNames,
                        Images = images.ToList()
                    });
                }
            }

            return new PaginatedResult<PlantIDSearchResultDTO>() 
            {
                Items = results,
            };
        }

        private async Task<PaginatedResult<PlantIDSearchResultDTO>> FetchAndCacheSearchResultsAsync(string searchTerm, string metadataKey, int page, int pageSize)
        {
            try
            {
                var paginatedResult = await _taxonomyRepository.Search(searchTerm, page, pageSize);
                var searchResults = paginatedResult.Items;
                var results = new ConcurrentBag<PlantIDSearchResultDTO>();
                var metadataList = new List<PlantMetadataDTO>();
                var semaphore = new SemaphoreSlim(10);

                int plantsWithoutImages = 0;

                if (paginatedResult.Items.Count() > 0)
                {
                    Console.WriteLine($"[PlantID] Found {searchResults.Count()} taxa for {searchTerm}.");
                    Console.WriteLine($"[PlantID] Getting images for {searchResults.Count()} records...");

                    var tasks = searchResults.Select(async item =>
                    {
                        await semaphore.WaitAsync();
                        try
                        {
                            var images = await _wikimediaService.GetImagesFromWikimediaAsync(item.ScientificName);
                            if (images != null && images.Any())
                            {
                                var commonNames = item.VernacularNames.Select(x => x.Name).ToList();
                                metadataList.Add(new PlantMetadataDTO
                                {
                                    ScientificName = item.ScientificName,
                                    CommonNames = commonNames,
                                    TaxonKey = item.TaxonKey
                                });

                                results.Add(new PlantIDSearchResultDTO
                                {
                                    ScientificName = item.ScientificName,
                                    CommonNames = commonNames,
                                    Images = images.ToList()
                                });

                                var imageKey = $"{PlantNetCacheKey}:image:{item.TaxonKey}";
                                await _cacheService.SetAsync(imageKey, images, TimeSpan.FromHours(1));
                            }
                            else
                            {
                                Interlocked.Increment(ref plantsWithoutImages);
                            }
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    });


                    await Task.WhenAll(tasks);

                    Console.WriteLine($"[PlantID] {plantsWithoutImages} plants did not have images.");
                    Console.WriteLine($"[PlantID] Caching {metadataList.Count()} records for {searchTerm}.");
                    await _cacheService.SetAsync(metadataKey, metadataList, TimeSpan.FromHours(1));
                }
                else
                {
                    Console.WriteLine($"[PlantID] no results found for {searchTerm}.");
                }

                return new PaginatedResult<PlantIDSearchResultDTO>
                {
                    Items = results.ToList(),
                    CurrentPage = paginatedResult.CurrentPage,
                    PageSize = paginatedResult.PageSize,
                    TotalItems = paginatedResult.TotalItems,
                    TotalPages = paginatedResult.TotalPages
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error encountered while searching: Error: {ex.Message}");
            }
        }

    }
}
