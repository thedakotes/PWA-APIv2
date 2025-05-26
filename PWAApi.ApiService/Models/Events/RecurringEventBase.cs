using PWAApi.ApiService.Enums;

namespace PWAApi.ApiService.Models.Events
{
    public abstract class RecurringEventBase : EventBase
    {
        /// <summary>
        /// Determines if Reminder is recurring.
        /// If true - we'll calculate subsequent occurrences
        /// </summary>
        public bool IsRecurring { get; set; }

        /// <summary>
        /// The chunk of time between occurrences
        /// </summary>
        public RecurrenceUnit RecurrenceUnit { get; set; } = RecurrenceUnit.None;

        /// <summary>
        /// The number of RecurrenceUnits between occurrences
        /// </summary>
        public int RecurrenceInterval { get; set; }

        /// <summary>
        /// <summary>
        /// Optional max number of times to repeat after the initial occurrence
        /// Null = infinite; 0 = only initial; 1 = initial + one repeat, etc.
        /// </summary>
        public int? RecurrenceCount { get; set; }

        /// <summary>
        /// How many occurrences have already been generated
        /// </summary>
        public int OccurrenceCounter { get; set; } = 0;

        /// <summary>
        /// The first "due" date
        /// If null it is effectively due immediately
        /// </summary>
        public DateTimeOffset? StartDate { get; set; }

        /// <summary>
        /// The date and time to stop repeating (null = forever)
        /// </summary>
        public DateTimeOffset? EndDate { get; set; }

        /// <summary>
        /// Compute the next due date *after* a given point.
        /// Returns null if no further occurrences remain.
        /// </summary>
        public DateTimeOffset? GetNextOccurrence(DateTimeOffset fromDate)
        {
            // Base for next occurrence
            var next = StartDate ?? CreatedOn;

            // If this is a repeat, jump to last generated
            if (OccurrenceCounter > 0)
            {
                next = RecurrenceUnit switch
                {
                    RecurrenceUnit.Day => next.AddDays(RecurrenceInterval * OccurrenceCounter),
                    RecurrenceUnit.Week => next.AddDays(RecurrenceInterval * 7 * OccurrenceCounter),
                    RecurrenceUnit.Month => next.AddMonths(RecurrenceInterval * OccurrenceCounter),
                    RecurrenceUnit.Year => next.AddYears(RecurrenceInterval * OccurrenceCounter),
                    _ => next
                };
            }

            // If we're not recurring, only return once
            if (!IsRecurring || RecurrenceUnit == RecurrenceUnit.None)
            {
                return next > fromDate ? next : null;
            }

            // Fast-forward until we're beyond 'fromDate'
            while (next <= fromDate)
            {
                next = RecurrenceUnit switch
                {
                    RecurrenceUnit.Day => next.AddDays(RecurrenceInterval),
                    RecurrenceUnit.Week => next.AddDays(RecurrenceInterval * 7),
                    RecurrenceUnit.Month => next.AddMonths(RecurrenceInterval),
                    RecurrenceUnit.Year => next.AddYears(RecurrenceInterval),
                    _ => throw new InvalidOperationException($"Unsupported unit {RecurrenceUnit}")
                };

                // Increment the counter for each generated occurrence
                OccurrenceCounter++;
            }

            return next;
        }
    }
}
