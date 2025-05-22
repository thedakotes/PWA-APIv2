using PWAApi.ApiService.Enums;

namespace PWAApi.ApiService.Models.Reminder
{
    public abstract class ReminderEntity : BaseEntity
    {
        /// <summary>
        /// A short description of the Reminder
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// An enum to track the level of priority
        /// </summary>
        public PriorityLevel PriorityLevel { get; set; } = PriorityLevel.None;

        /// <summary>
        /// Tracks if the Reminder has been completed
        /// </summary>
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                if (!_isCompleted && value)
                {
                    CompletedOn = DateTime.UtcNow;
                }
                else if (_isCompleted && !value)
                {
                    CompletedOn = null;
                }

                _isCompleted = value;
            }
        }
        private bool _isCompleted = false;

        /// <summary>
        /// The date and time that Reminder was marked as completed
        /// </summary>
        public DateTimeOffset? CompletedOn { get; set; }
    }
}
