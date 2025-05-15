using EventApi.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

public class EventRepository : Repository<Event>, IEventRepository
{
    public EventRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Event>> GetByDateAsync(DateTime date)
    {
        return await _dbSet.Where(x => x.Date.Date == date.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetByUser(Guid userID)
    {
        return await _dbSet.Where(x => x.UserID == userID)
            .ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetUpcomingEvents()
    {
        return await _dbSet.Where(x => x.Date >= DateTime.UtcNow).ToListAsync(); // Asynchronously fetch all events from the database
    }
}