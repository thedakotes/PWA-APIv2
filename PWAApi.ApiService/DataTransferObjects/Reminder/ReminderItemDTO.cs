namespace PWAApi.ApiService.DataTransferObjects.Reminder
{
    public record ReminderItemDTO(int Id, 
        DateTimeOffset CreatedOn,
        DateTimeOffset? UpdatedOn, 
        string Description, 
        string? Url) : BaseEntityDTO(Id,
        CreatedOn,
        UpdatedOn);
}
