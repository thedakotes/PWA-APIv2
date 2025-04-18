using PWAApi.ApiService.DataTransferObjects.PlantID;

namespace PWAApi.ApiService.Models.PlantID
{
    public class PlantIDResponse
    {
        public required PlantIDSearchResult[] Items { get; set; }
    }

    public class PlantIDSearchResult
    {
        public required string ScientificName { get; set; }
        public string? CommonName { get; set; }
    }
}
