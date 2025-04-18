using Microsoft.AspNetCore.Mvc;
using PWAApi.ApiService.Services;

namespace PWAApi.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WateringScheduleController : ControllerBase
    {
        protected readonly WateringScheduleService _wateringScheduleService;

        public WateringScheduleController(WateringScheduleService wateringScheduleService)
        {
            _wateringScheduleService = wateringScheduleService;
        }

        [HttpGet("{species}")]
        public async Task<IActionResult> Get(string species)
        {
            if (string.IsNullOrEmpty(species))
            {
                return BadRequest("No search term provided");
            }

            try
            {
                var result = await _wateringScheduleService.GetSuggestedIndoorWateringSchedule(species);
                return Ok(result);
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
