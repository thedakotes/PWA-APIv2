namespace PWAApi.ApiService.Models.Events
{
    public class CalendarEvent : EventBase
    {
        /// <summary>
        /// The hex string of the color of the event
        /// </summary>
        public string? Color { get; set; } = string.Empty;

        /// <summary>
        /// The date the event will occur
        /// </summary>
        public DateTimeOffset OccurrenceDate { get; set; }
    }
}
