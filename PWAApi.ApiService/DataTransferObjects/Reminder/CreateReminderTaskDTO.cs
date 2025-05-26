namespace PWAApi.ApiService.DataTransferObjects.Reminder
{
    public record CreateReminderTaskDTO(string Description, bool isCompleted, string? Url);
}
