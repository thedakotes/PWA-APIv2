using AutoMapper;
using API.DataTransferObjects;
using OpenAI.Chat;
using PWAApi.ApiService.Services.AI;
using PWAApi.ApiService.Helpers;
using PWAApi.ApiService.Services;
using PWAApi.ApiService.Authentication.Models;
using PWAApi.ApiService.Models.Events;

public class CalendarEventService : EntityService<CalendarEvent, CalendarEventDTO, CalendarEventDTO, ICalendarEventRepository>, ICalendarEventService
{
    private readonly IAIService _aiService;
    private readonly ICurrentUser _currentUser;

    public CalendarEventService(ICurrentUser currentUser,
        ICalendarEventRepository calendarEventRepository, 
        IMapper mapper,
        IAIService aiService) : base(mapper, calendarEventRepository)
    {
        _aiService = aiService;
        _currentUser = currentUser;
    }

    public async Task<List<CalendarEventDTO>> AICreateEvents(string eventDetails)
    {
        ChatCompletionOptions options = OpenAIHelper.SetChatCompletionOptions<CalendarEventDTO>("event_parsing");
        var userMessages = OpenAIHelper.SetUserChatMessages(new List<string>() { $"Create an event for: '{eventDetails}'." });
        var result = await _aiService.Ask<CalendarEventDTO>(options, userMessages.Cast<ChatMessage>().ToList());

        return [result];
    }

    public async Task<IEnumerable<CalendarEventDTO>> GetByUser()
    {
        try
        {
            var models = await _repository.GetByUser(_currentUser.UserID);
            var modelDTOs = models.Select(x => _mapper.Map<CalendarEventDTO>(x));
            return modelDTOs;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
}