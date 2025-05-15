using PWAApi.ApiService.Enums;

namespace PWAApi.ApiService.DataTransferObjects
{
    public record ReminderDTO(int id,
        string? Description,
        string? Notes,
        PriorityLevelEnum PriorityLevel,
        bool isCompleted,
        DateTimeOffset? CompletedOn);
}
