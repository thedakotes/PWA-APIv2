using System.ComponentModel.DataAnnotations.Schema;

namespace PWAApi.ApiService.Models.Events.Reminder
{
    public class ReminderTask : EventTaskBase
    {
        /// <summary>
        /// Reference to the parent Reminder
        /// </summary>
        [ForeignKey("ReminderID")]
        public required virtual Reminder Reminder { get; set; }
    }
}
