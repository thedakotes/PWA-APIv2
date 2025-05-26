using PWAApi.ApiService.Models.Events;

public interface ICalendarEventRepository : IRepository<CalendarEvent>
{
    // Get events by a specific date
    Task<IEnumerable<CalendarEvent>> GetByDateAsync(DateTime date);

    // Get for User
    Task<IEnumerable<CalendarEvent>> GetByUser(Guid userID);

    Task<IEnumerable<CalendarEvent>> GetUpcomingEvents();
}