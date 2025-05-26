using PWAApi.ApiService.Models.Events.Reminder;

namespace PWAApi.ApiService.Repositories.Event
{
    public interface IReminderRepository : IRepository<Reminder>
    {
        Task<IEnumerable<Reminder>> GetByUser(Guid userID);
    }
}
