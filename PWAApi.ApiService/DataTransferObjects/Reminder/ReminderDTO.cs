using PWAApi.ApiService.Enums;

namespace PWAApi.ApiService.DataTransferObjects.Reminder
{
    public record ReminderDTO(int Id,
        DateTimeOffset CreatedOn,
        DateTimeOffset? UpdatedOn,
        bool IsCompleted,
        DateTimeOffset? CompletedOn,
        DateTimeOffset? NextOccurrence = default,
        IReadOnlyList<ReminderItemDTO>? Items = null,
        IReadOnlyList<ReminderTaskDTO>? Tasks = null) : CreateReminderDTO;
}
