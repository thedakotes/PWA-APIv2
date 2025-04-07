using Microsoft.AspNetCore.Mvc;

namespace PWAApi.ApiService.DataTransferObjects.PlantID
{
    public class PlantIDRequestDTO
    {
        public required List<IFormFile> Files { get; set; }

        public string Organ { get; set; } = "flower";
    }
}
