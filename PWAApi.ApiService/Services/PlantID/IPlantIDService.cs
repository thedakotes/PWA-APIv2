using AutoMapper;
using PWAApi.ApiService.DataTransferObjects.PlantID;

namespace API.Services.PlantID
{
    public interface IPlantIDService
    {
        Task<IEnumerable<PlantIDDTO>> IdentifyPlantAsync(List<IFormFile> files);
    }
}
