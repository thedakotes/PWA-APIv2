using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PWAApi.ApiService.DataTransferObjects.Reminder;
using PWAApi.ApiService.Services;

namespace PWAApi.ApiService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReminderController : ControllerBase
    {
        private readonly IReminderService _reminderService;
        private readonly IReminderTaskService _reminderTaskService;

        public ReminderController(IReminderService reminderService, IReminderTaskService reminderTaskService)
        {
            _reminderService = reminderService;
            _reminderTaskService = reminderTaskService;
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
                return CreatedAtAction(
                    nameof(Get),
                    new { id = newReminder.Id},
                    newReminder
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("AddItem{id:int}")]
        public async Task<IActionResult> AddItem(int id, [FromBody] CreateReminderItemDTO dataTransferObject)
        {
            try
            {
                var newReminderItem = await _reminderService.AddItem(id, dataTransferObject.Description, dataTransferObject.Url);
                return CreatedAtAction(
                    nameof(Get),
                    new { id }, 
                    newReminderItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("AddTask{id:int}")]
        public async Task<IActionResult> AddTask(int id, [FromBody] CreateReminderTaskDTO dataTransferObject)
        {
            try
            {
                var newReminderTask = await _reminderService.AddTask(id, dataTransferObject.Description, dataTransferObject.isCompleted, dataTransferObject.Url);
                return CreatedAtAction(
                    nameof(Get),
                    new { id },
                    newReminderTask);
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

        [HttpPut("CompleteTask{id:int}")]
        public async Task<IActionResult> CompleteTask(int id)
        {
            try
            {
                var reminderTask = await _reminderTaskService.Complete(id);
                return Ok(reminderTask);
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
