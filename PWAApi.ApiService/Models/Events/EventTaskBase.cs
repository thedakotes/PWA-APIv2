namespace PWAApi.ApiService.Models.Events
{
    public abstract class EventTaskBase : EventItemBase, ICompletable
    {
        public DateTimeOffset? CompletedOn { get; set; }

        public bool IsCompleted { get; set; }
    }
}
