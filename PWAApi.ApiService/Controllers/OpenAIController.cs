using Microsoft.AspNetCore.Mvc;
using PWAApi.ApiService.Services;

namespace PWAApi.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OpenAIController : ControllerBase
    {

        private readonly OpenAIService _openAIService;

        public OpenAIController(OpenAIService openAIService)
        {
            _openAIService = openAIService ?? throw new ArgumentNullException(nameof(openAIService));
        }

        [HttpGet("AskAI")]
        public async Task<IActionResult> AskAI(string question)
        {
            try
            {
                var result = await _openAIService.AskAI(question);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
