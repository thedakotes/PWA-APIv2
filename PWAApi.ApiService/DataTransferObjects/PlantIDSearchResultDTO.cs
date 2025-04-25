using PWAApi.ApiService.DataTransferObjects.PlantID;

namespace PWAApi.ApiService.DataTransferObjects
{
    public class PlantIDSearchResultDTO
    {
        public string ScientificName { get; set; } = string.Empty;
        public string? CommonName { get; set; }
        public List<ImageDTO>? Images { get; set; }

        public PlantIDSearchResultDTO(string scientificName, string commonName) 
        {
            ScientificName = scientificName;
            CommonName = commonName;
        }

    }
}
