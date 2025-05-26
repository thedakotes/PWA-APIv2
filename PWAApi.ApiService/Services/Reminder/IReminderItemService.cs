using PWAApi.ApiService.DataTransferObjects.Reminder;
using PWAApi.ApiService.Models.Events.Reminder;

namespace PWAApi.ApiService.Services
{
    public interface IReminderItemService : IEntityService<ReminderItem, ReminderItemDTO, CreateReminderItemDTO>
    {
    }
}
