using System.Globalization;
using API.Services.PlantID;
using CsvHelper;
using CsvHelper.Configuration;
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

        [HttpGet("{species}")]
        public async Task<IActionResult> Get(string species)
        {
            if (String.IsNullOrEmpty(species))
            {
                return BadRequest("No search term provided");
            }

            try
            {
                var result = await _plantInfoService.GetPlantAsync(species);
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

        [HttpGet("Identify/{species}")]
        public async Task<IActionResult> Identify(string species)
        {
            if (string.IsNullOrEmpty(species))
            {
                return BadRequest("No species provided.");
            }

            try
            {
                var result = await _plantIDService.IdentifyPlantAsync(species);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}