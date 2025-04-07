using PWAApi.ApiService.DataTransferObjects.PlantID;

namespace API.Services.PlantID
{
    public interface IPlantInfoService
    {
        Task<IEnumerable<PlantDTO>> GetPlantSpeciesAsync(string name);
    }
}
