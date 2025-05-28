using PWAApi.ApiService.Enums;

namespace PWAApi.ApiService.DataTransferObjects.Reminder
{
    public record ReminderTaskDTO(int Id,
        int ReminderId,
        DateTimeOffset CreatedOn,
        DateTimeOffset? UpdatedOn,
        string Description,
        PriorityLevel PriorityLevel,
        bool IsCompleted,
        DateTimeOffset? CompletedOn,
        string? Url);
}
