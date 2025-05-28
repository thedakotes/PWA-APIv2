namespace PWAApi.ApiService.Models.Events.Reminder
{
    public class ReminderItem : EventItemBase
    {
        /// <summary>
        /// Foreign key to the parent Reminder
        /// </summary>
        public required int ReminderId { get; set; }

        /// <summary>
        /// Reference to the parent Reminder
        /// </summary>
        public virtual Reminder? Reminder { get; set; }
    }
}
