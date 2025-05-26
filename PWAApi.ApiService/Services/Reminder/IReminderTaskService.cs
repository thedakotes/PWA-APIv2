using PWAApi.ApiService.DataTransferObjects.Reminder;
using PWAApi.ApiService.Models.Events.Reminder;

namespace PWAApi.ApiService.Services
{
    public interface IReminderTaskService : IEntityService<ReminderTask, ReminderTaskDTO, CreateReminderTaskDTO>
    {
        public Task<ReminderTaskDTO> Complete(int id);
    }
}
