using AutoMapper;
using PWAApi.ApiService.DataTransferObjects.Reminder;
using PWAApi.ApiService.Models.Events.Reminder;
using PWAApi.ApiService.Repositories.Event;

namespace PWAApi.ApiService.Services
{
    public class ReminderItemService : EntityService<ReminderItem, ReminderItemDTO, CreateReminderItemDTO, IReminderItemRepository>, IReminderItemService
    {
        public ReminderItemService(IMapper mapper, IReminderItemRepository repository)
            : base(mapper, repository)
        {
        }
    }
}
