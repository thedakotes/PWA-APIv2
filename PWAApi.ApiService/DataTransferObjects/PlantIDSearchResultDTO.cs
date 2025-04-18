using PWAApi.ApiService.DataTransferObjects.PlantID;

namespace PWAApi.ApiService.DataTransferObjects
{
    public class PlantIDSearchResultDTO
    {
        public required string ScientificName { get; set; }
        public string? CommonName { get; set; }
        public ImageDTO? Image { get; set; }
    }
}
