using EventApi.Data;
using Microsoft.EntityFrameworkCore;
using PWAApi.ApiService.Models.Events;

public class EventRepository : Repository<CalendarEvent>, ICalendarEventRepository
{
    public EventRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<CalendarEvent>> GetByDateAsync(DateTime date)
    {
        return await _dbSet.Where(x => x.OccurrenceDate.Date == date.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<CalendarEvent>> GetByUser(Guid userID)
    {
        return await _dbSet.Where(x => x.UserId == userID)
            .ToListAsync();
    }

    public async Task<IEnumerable<CalendarEvent>> GetUpcomingEvents()
    {
        return await _dbSet.Where(x => x.OccurrenceDate.Date >= DateTime.UtcNow).ToListAsync(); // Asynchronously fetch all events from the database
    }
}