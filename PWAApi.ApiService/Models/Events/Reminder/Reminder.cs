using PWAApi.ApiService.Enums;

namespace PWAApi.ApiService.Models.Events.Reminder
{
    public class Reminder : RecurringEventBase, ICompletable, IUserAssociated
    {
        public DateTimeOffset? CompletedOn { get; set; }

        public bool IsCompleted { get; set; }

        public PriorityLevel PriorityLevel { get; set; } = PriorityLevel.Low;

        /// <summary>
        /// Collection of associated Items containing extra data
        /// </summary>
        public virtual ICollection<ReminderItem> Items { get; set; } = new HashSet<ReminderItem>();

        /// <summary>
        /// Collection of associated Tasks to complete 
        /// </summary>
        public virtual ICollection<ReminderTask> Tasks { get; set; } = new HashSet<ReminderTask>();

        /// <summary>
        /// Add a new Item to the Reminder
        /// </summary>
        /// <param name="description"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public ReminderItem AddItem(string description, string? url)
        {
            var item = new ReminderItem
            {
                Description = description,
                Url = url,
                Reminder = this
            };

            Items.Add(item);
            return item;
        }

        /// <summary>
        /// Add a new Task to the Reminder
        /// </summary>
        /// <param name="description"></param>
        /// <param name="isCompleted"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        public ReminderTask AddTask(string description, bool isCompleted, string? Url)
        {
            var task = new ReminderTask
            {
                Description = description,
                IsCompleted = isCompleted,
                Url = Url,
                Reminder = this
            };

            Tasks.Add(task);
            return task;
        }
    }
}
