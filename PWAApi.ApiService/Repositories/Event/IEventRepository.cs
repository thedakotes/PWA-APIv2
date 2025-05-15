using API.Models;

public interface IEventRepository : IRepository<Event>
{
    // Get events by a specific date
    Task<IEnumerable<Event>> GetByDateAsync(DateTime date);

    // Get for User
    Task<IEnumerable<Event>> GetByUser(Guid userID);

    Task<IEnumerable<Event>> GetUpcomingEvents();
}