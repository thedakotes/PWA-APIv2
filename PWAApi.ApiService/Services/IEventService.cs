using API.DataTransferObjects;
using API.Models;

public interface IEventService
{
    Task<EventDTO>AddEventAsync(EventDTO newEvent);
    Task<IEnumerable<EventDTO>> GetAllEventsAsync();
    Task<EventDTO?> GetEventByIdAsync(int id);
    Task DeleteEventAsync(int id);
    Task UpdateEventAsync(EventDTO updatedEvent);
}