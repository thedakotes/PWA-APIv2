namespace PWAApi.ApiService.DataTransferObjects.Reminder
{
    public record ReminderItemDTO(int Id,
        int ReminderId,
        DateTimeOffset CreatedOn,
        DateTimeOffset? UpdatedOn, 
        string Description, 
        string? Url);
}
