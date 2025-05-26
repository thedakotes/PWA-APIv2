using EventApi.Data;
using Microsoft.EntityFrameworkCore;
using PWAApi.ApiService.Models.Events.Reminder;

namespace PWAApi.ApiService.Repositories.Event
{
    public class ReminderRepository : Repository<Reminder>, IReminderRepository
    {
        public ReminderRepository(AppDbContext appDbContext) : base(appDbContext) { }

        public async Task<IEnumerable<Reminder>> GetByUser(Guid userID)
        {
            return await _dbSet.Where(x => x.UserId == userID).ToListAsync();
        }
    }
}
