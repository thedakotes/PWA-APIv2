using AutoMapper;
using PWAApi.ApiService.DataTransferObjects.PlantID;
using PWAApi.ApiService.Models;

namespace API.Services.PlantID
{
    public interface IPlantIDService
    {
        Task<IEnumerable<PlantIDDTO>> IdentifyPlantAsync(List<IFormFile> files);
        Task<PaginatedResult<PlantIDSearchResultDTO>?> IdentifyPlantAsync(string species, int page, int pageSize);
    }
}
