using PWAApi.ApiService.DataTransferObjects.Reminder;
using PWAApi.ApiService.Models.Events.Reminder;

namespace PWAApi.ApiService.Services
{
    public interface IReminderService : IEntityService<Reminder, ReminderDTO, CreateReminderDTO>
    {
        public Task<ReminderItemDTO> AddItem(int reminderId, string description, string? url);
        public Task<ReminderTaskDTO> AddTask(int reminderId, string description, bool isCompleted, string? url);
        public Task<ReminderDTO> Complete(int id);
        public Task<IEnumerable<ReminderDTO>> GetByUser();
    }
}
