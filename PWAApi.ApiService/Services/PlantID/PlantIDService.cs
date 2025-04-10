using PWAApi.ApiService.DataTransferObjects.PlantID;
using PWAApi.ApiService.Models.PlantID;

namespace API.Services.PlantID
{
    public class PlantIDService : IPlantIDService
    {
        public Task<IEnumerable<PlantIDDTO>> IdentifyPlantAsync(List<IFormFile> file)
        {
            throw new NotImplementedException();
        }
    }
}
