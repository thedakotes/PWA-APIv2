using API.DataTransferObjects;
using PWAApi.ApiService.Models.Events;
using PWAApi.ApiService.Services;

public interface ICalendarEventService : IEntityService<CalendarEvent, CalendarEventDTO, CalendarEventDTO>
{
    Task<List<CalendarEventDTO>> AICreateEvents(string eventDetails);

    Task<IEnumerable<CalendarEventDTO>> GetByUser();
}