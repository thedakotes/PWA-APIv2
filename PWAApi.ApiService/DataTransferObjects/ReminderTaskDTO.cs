using PWAApi.ApiService.Enums;

namespace PWAApi.ApiService.DataTransferObjects
{
    public record ReminderTaskDTO(int ID,
        string? Description,
        PriorityLevel PriorityLevel,
        bool IsCompleted,
        DateTimeOffset? CompletedOn,
        string? Url);
}
