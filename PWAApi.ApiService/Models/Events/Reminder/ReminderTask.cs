using System.ComponentModel.DataAnnotations.Schema;
using PWAApi.ApiService.Enums;

namespace PWAApi.ApiService.Models.Events.Reminder
{
    public class ReminderTask : EventTaskBase
    {
        /// <summary>
        /// User determined level of priority
        /// </summary>
        public PriorityLevel PriorityLevel { get; set; } = PriorityLevel.Low;

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
