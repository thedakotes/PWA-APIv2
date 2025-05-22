using PWAApi.ApiService.Enums;

namespace PWAApi.ApiService.DataTransferObjects
{
    public record CreateReminderDTO(
        string? Description,
        string? Notes,
        PriorityLevel PriorityLevel,
        bool IsRecurring,
        RecurrenceUnit? RecurrenceUnit,
        int? RecurrenceInterval,
        int? RecurrenceCount,
        DateTimeOffset? EndDate,
        DateTimeOffset? StartDate);

    public record ReminderDTO(int ID,
        string? Description,
        string? Notes,
        PriorityLevel PriorityLevel,
        bool IsCompleted,
        DateTimeOffset? CompletedOn,
        bool IsRecurring,
        RecurrenceUnit? RecurrenceUnit,
        int? RecurrenceInterval,
        int? RecurrenceCount,
        int? OccurrenceCounter,
        DateTimeOffset? EndDate,
        DateTimeOffset? StartDate,
        DateTimeOffset? NextOccurrence = default,
        IReadOnlyList<ReminderTaskDTO>? Tasks = null);
}
