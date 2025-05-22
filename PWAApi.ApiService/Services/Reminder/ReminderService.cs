using AutoMapper;
using PWAApi.ApiService.Authentication.Models;
using PWAApi.ApiService.DataTransferObjects;
using PWAApi.ApiService.Models.Reminder;
using PWAApi.ApiService.Repositories;

namespace PWAApi.ApiService.Services
{
    public class ReminderService : EntityService<Reminder, ReminderDTO, CreateReminderDTO, IReminderRepository>, IReminderService
    {
        private readonly ICurrentUser _currentUser;

        public ReminderService(ICurrentUser currentUser, IMapper mapper, IReminderRepository repository) : base(mapper, repository)
        {
            _currentUser = currentUser;
        }

        public async Task<ReminderDTO> Complete(int id)
        {
            try
            {
                var reminder = await _repository.GetByIdAsync(id);
                if (reminder == null)
                {
                    throw new Exception($"Reminder not found");
                }

                foreach (var reminderTask in reminder.Tasks)
                {
                    reminderTask.IsCompleted = true;
                }
                reminder.IsCompleted = true;

                await _repository.UpdateAsync(reminder);
                return _mapper.Map<ReminderDTO>(reminder);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<ReminderDTO>> GetByUser()
        {
            try
            {
                var models = await _repository.GetByUser(_currentUser.UserID);
                var modelDTOs = models.Select(x => _mapper.Map<ReminderDTO>(x));
                return modelDTOs.OrderBy(x => x.IsCompleted).ThenByDescending(x => x.PriorityLevel).ThenByDescending(x => x.ID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
