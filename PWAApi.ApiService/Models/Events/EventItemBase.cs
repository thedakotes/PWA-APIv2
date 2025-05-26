namespace PWAApi.ApiService.Models.Events
{
    public abstract class EventItemBase : BaseEntity
    {
        /// <summary>
        /// A short description
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// Url linking to a site to find instructions, buy a product, etc.
        /// </summary>
        public string? Url { get; set; }
    }
}
