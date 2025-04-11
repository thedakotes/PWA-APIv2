using PWAApi.ApiService.DataTransferObjects.PlantID;

namespace API.Services.PlantID
{
    public interface IPlantInfoService
    {
        Task<IEnumerable<PlantDTO?>> GetPlantSpeciesAsync(string species);

        Task<PlantDTO?> GetPlantAsync(string species);
    }
}
