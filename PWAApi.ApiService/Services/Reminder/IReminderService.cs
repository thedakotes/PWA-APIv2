using PWAApi.ApiService.DataTransferObjects;
using PWAApi.ApiService.Models.Reminder;

namespace PWAApi.ApiService.Services
{
    public interface IReminderService : IEntityService<Reminder, ReminderDTO, CreateReminderDTO>
    {
        public Task<ReminderDTO> Complete(int id);
        public Task<IEnumerable<ReminderDTO>> GetByUser();
    }
}
