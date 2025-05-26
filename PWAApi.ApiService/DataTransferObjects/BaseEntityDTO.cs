namespace PWAApi.ApiService.DataTransferObjects
{
    public record BaseEntityDTO(int Id, DateTimeOffset CreatedOn, DateTimeOffset? UpdatedOn);
}
