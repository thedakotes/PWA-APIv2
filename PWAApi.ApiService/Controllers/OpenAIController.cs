using Microsoft.AspNetCore.Mvc;
using PWAApi.ApiService.Services.AI;

namespace PWAApi.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OpenAIController : ControllerBase
    {

        private readonly IAIService _aiService;

        public OpenAIController(IAIService aiService)
        {
            _aiService = aiService ?? throw new ArgumentNullException(nameof(aiService));
        }

        [HttpGet("AskAI")]
        public async Task<IActionResult> AskAI(string question)
        {
            try
            {
                var result = await _aiService.AskAI(question);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
