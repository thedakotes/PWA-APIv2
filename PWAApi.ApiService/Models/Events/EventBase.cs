namespace PWAApi.ApiService.Models.Events
{
    public abstract class EventBase : BaseEntity, IUserAssociated
    {
        /// <summary>
        /// A short description of the event
        /// </summary>
        public string? Description { get; set; } = string.Empty;

        /// <summary>
        /// A title for the event
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// The Guid for the User who generated the event
        /// </summary>
        public required Guid UserId { get; set; }
    }
}