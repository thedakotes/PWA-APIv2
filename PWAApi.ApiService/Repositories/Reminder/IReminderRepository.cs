using PWAApi.ApiService.Models;

namespace PWAApi.ApiService.Repositories
{
    public interface IReminderRepository : IRepository<Reminder>
    {
        Task<IEnumerable<Reminder>> GetByUser(Guid userID);
    }
}
