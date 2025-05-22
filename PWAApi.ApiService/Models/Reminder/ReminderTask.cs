using System.ComponentModel.DataAnnotations.Schema;

namespace PWAApi.ApiService.Models.Reminder
{
    public class ReminderTask : ReminderEntity
    {
        /// <summary>
        /// A link to a site to purchase something, find some information, etc.
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// Reference to the parent Reminder
        /// </summary>
        [ForeignKey("ReminderID")]
        public required virtual Reminder Reminder { get; set; }
    }
}
