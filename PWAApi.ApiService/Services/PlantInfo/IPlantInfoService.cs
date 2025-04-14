using PWAApi.ApiService.DataTransferObjects.PlantID;

namespace PWAApi.ApiService.Services.PlantInfo
{
    public interface IPlantInfoService
    {
        Task<PlantDTO?> GetPlantAsync(string species);
    }
}
