using API.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventsService;

    public EventsController(IEventService eventsService)
    {
        _eventsService = eventsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetEvents()
    {
        try
        {
            // Retrieve all events from the service
            var events = await _eventsService.GetAllEventsAsync();
            return Ok(events); // Return a 200 OK response with the list of events
        }
        catch (Exception ex)
        {
            // Handle any exceptions and return a 500 Internal Server Error response
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetEvent(int id)
    {
        try
        {
            // Retrieve all events from the service
            var eventModel = await _eventsService.GetEventByIdAsync(id);
            return Ok(eventModel); // Return a 200 OK response with the event
        }
        catch (Exception ex)
        {
            // Handle any exceptions and return a 500 Internal Server Error response
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddEvent([FromBody] EventDTO newEvent)
    {
        if (newEvent == null)
        {
            return BadRequest("Event data is null."); // Return a 400 Bad Request if the input is null
        }

        try
        {
            // Add the new event using the service
            EventDTO entity = await _eventsService.AddEventAsync(newEvent);
            return Ok(entity); // Return a 201 Created response
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}"); // Handle exceptions
        }
    }

    [HttpPost("AICreateEvents")]
    public async Task<IActionResult> AICreateEvents(string message)
    {
       
        try
        {
            //This could potentially be a number of different events that get sent in? I haven't tried this yet tho.
            List<EventDTO> events = await _eventsService.AICreateEvents(message);
            return Ok(events); // Return a 200
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}"); // Handle exceptions
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        try
        {
            if (await _eventsService.GetEventByIdAsync(id) != null)
            {
                await _eventsService.DeleteEventAsync(id);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}