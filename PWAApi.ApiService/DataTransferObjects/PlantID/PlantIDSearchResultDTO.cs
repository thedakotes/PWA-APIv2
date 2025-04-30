namespace PWAApi.ApiService.DataTransferObjects.PlantID
{
    public class PlantIDSearchResultDTO
    {
        public string ScientificName { get; set; } = string.Empty;
        public List<string> CommonNames { get; set; } = new List<string>();
        public List<ImageDTO>? Images { get; set; }

    }
}
