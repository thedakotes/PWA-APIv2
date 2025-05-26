using EventApi.Data;
using PWAApi.ApiService.Models.Events.Reminder;

namespace PWAApi.ApiService.Repositories.Event
{
    public class ReminderItemRepository : Repository<ReminderItem>, IReminderItemRepository
    {
        public ReminderItemRepository(AppDbContext appDbContext) : base(appDbContext) { }
    }
}
