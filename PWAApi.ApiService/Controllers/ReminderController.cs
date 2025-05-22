using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PWAApi.ApiService.DataTransferObjects;
using PWAApi.ApiService.Services;

namespace PWAApi.ApiService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReminderController : ControllerBase
    {
        private readonly IReminderService _reminderService;

        public ReminderController(IReminderService reminderService)
        {
            _reminderService = reminderService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var reminder = await _reminderService.Get(id);
                return Ok(reminder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetByUser")]
        public async Task<IActionResult> GetByUser()
        {
            try
            {
                var reminders = await _reminderService.GetByUser();
                return Ok(reminders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] CreateReminderDTO dataTransferObject)
        {
            try
            {
                var newReminder = await _reminderService.Add(dataTransferObject);
                return Ok(newReminder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("Complete{id:int}")]
        public async Task<IActionResult> Complete(int id)
        {
            try
            {
                var reminder = await _reminderService.Complete(id);
                return Ok(reminder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] ReminderDTO dataTransferObject)
        {
            try
            {
                var updatedReminder = await _reminderService.Update(dataTransferObject);
                return Ok(updatedReminder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _reminderService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
