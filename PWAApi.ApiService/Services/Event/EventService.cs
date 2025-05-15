using AutoMapper;
using API.DataTransferObjects;
using API.Models;
using OpenAI.Chat;
using PWAApi.ApiService.Services.AI;
using PWAApi.ApiService.Helpers;
using PWAApi.ApiService.Services;
using PWAApi.ApiService.Authentication.Models;

public class EventService : EntityService<Event, EventDTO, IEventRepository>, IEventService
{
    private readonly IAIService _aiService;
    private readonly ICurrentUser _currentUser;

    public EventService(ICurrentUser currentUser,
        IEventRepository eventRepository, 
        IMapper mapper,
        IAIService aiService) : base(mapper, eventRepository)
    {
        _aiService = aiService;
        _currentUser = currentUser;
    }

    public async Task<List<EventDTO>> AICreateEvents(string eventDetails)
    {
        ChatCompletionOptions options = OpenAIHelper.SetChatCompletionOptions<EventDTO>("event_parsing");
        var userMessages = OpenAIHelper.SetUserChatMessages(new List<string>() { $"Create an event for: '{eventDetails}'." });
        var result = await _aiService.Ask<EventDTO>(options, userMessages.Cast<ChatMessage>().ToList());

        return [result];
    }

    public async Task<IEnumerable<EventDTO>> GetByUser()
    {
        try
        {
            var models = await _repository.GetByUser(_currentUser.UserID);
            var modelDTOs = models.Select(x => _mapper.Map<EventDTO>(x));
            return modelDTOs;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
}