using AutoMapper;
using PWAApi.ApiService.DataTransferObjects.PlantID;

namespace API.Services.PlantID
{
    public interface IPlantIDService
    {
        Task<List<PlantIDDTO>> IdentifyPlantAsync(List<IFormFile> files);
    }
}
