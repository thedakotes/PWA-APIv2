using API.Services.PlantID;
using Microsoft.AspNetCore.Mvc;
using PWAApi.ApiService.DataTransferObjects.PlantID;
using PWAApi.ApiService.Services.PlantInfo;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantIDController : ControllerBase
    {
        IPlantIDService _plantIDService;
        IPlantInfoService _plantInfoService;

        public PlantIDController(IPlantIDService plantIDService, IPlantInfoService plantInfoService)
        {
            _plantIDService = plantIDService;
            _plantInfoService = plantInfoService;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return BadRequest("No search term provided");
            }

            try
            {
                var result = await _plantInfoService.GetPlantAsync(name);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("Identify")]
        public async Task<IActionResult> Identify([FromForm] PlantIDRequestDTO plantID)
        {
            if (plantID.Files == null || !plantID.Files.Any())
            {
                return BadRequest("No file uploaded");
            }

            try
            {
                var result = await _plantIDService.IdentifyPlantAsync(plantID.Files);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}