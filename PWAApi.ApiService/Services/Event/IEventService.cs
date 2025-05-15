using API.DataTransferObjects;
using API.Models;
using PWAApi.ApiService.Services;

public interface IEventService : IEntityService<Event, EventDTO>
{
    Task<List<EventDTO>> AICreateEvents(string eventDetails);

    Task<IEnumerable<EventDTO>> GetByUser();
}