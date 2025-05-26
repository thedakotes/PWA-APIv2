using EventApi.Data;
using PWAApi.ApiService.Models.Events.Reminder;

namespace PWAApi.ApiService.Repositories.Event
{
    public class ReminderTaskRepository : Repository<ReminderTask>, IReminderTaskRepository
    {
        public ReminderTaskRepository(AppDbContext appDbContext) : base(appDbContext) { }
    }
}
