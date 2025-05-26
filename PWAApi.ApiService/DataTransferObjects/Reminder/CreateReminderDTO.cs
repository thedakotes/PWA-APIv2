using PWAApi.ApiService.Enums;

namespace PWAApi.ApiService.DataTransferObjects.Reminder
{
    public record CreateReminderDTO()
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public PriorityLevel PriorityLevel { get; set; }
        public bool IsRecurring { get; set; }
        public RecurrenceUnit? RecurrenceUnit { get; set; }
        public int? RecurrenceInterval { get; set; }
        public int? RecurrenceCount { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public DateTimeOffset? StartDate { get; set; }
    }
}
