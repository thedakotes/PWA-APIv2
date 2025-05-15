using PWAApi.ApiService.DataTransferObjects;
using PWAApi.ApiService.Models;

namespace PWAApi.ApiService.Services
{
    public interface IReminderService : IEntityService<Reminder, ReminderDTO>
    {
        public Task<ReminderDTO> Complete(int id);
        public Task<IEnumerable<ReminderDTO>> GetByUser();
    }
}
