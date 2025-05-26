namespace PWAApi.ApiService.Models.Events
{
    public interface ICompletable
    {
        /// <summary>
        /// The date and time the entity was marked as completed
        /// </summary>
        DateTimeOffset? CompletedOn { get; set; }

        /// <summary>
        /// Tracks if the entity is completed
        /// </summary>
        bool IsCompleted { get; set; }
    }
}
