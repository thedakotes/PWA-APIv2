using PWAApi.ApiService.Enums;

namespace PWAApi.ApiService.DataTransferObjects.Reminder
{
    public record ReminderTaskDTO(int Id,
        DateTimeOffset CreatedOn,
        DateTimeOffset? UpdatedOn,
        string Description,
        bool IsCompleted,
        DateTimeOffset? CompletedOn,
        string? Url) : BaseEntityDTO(Id,
        CreatedOn,
        UpdatedOn);
}
