using AutoMapper;
using PWAApi.ApiService.Authentication.Models;
using PWAApi.ApiService.DataTransferObjects.Reminder;
using PWAApi.ApiService.Models.Events.Reminder;
using PWAApi.ApiService.Repositories.Event;

namespace PWAApi.ApiService.Services
{
    public class ReminderService : EntityService<Reminder, ReminderDTO, CreateReminderDTO, IReminderRepository>, IReminderService
    {
        private readonly ICurrentUser _currentUser;

        public ReminderService(ICurrentUser currentUser, IMapper mapper, IReminderRepository repository) : base(mapper, repository)
        {
            _currentUser = currentUser;
        }

        public async Task<ReminderItemDTO> AddItem(int id, string description, string? url)
        {
            // Check if we have associated Reminder
            var reminder = await _repository.GetByIdAsync(id);
            if (reminder == null)
            {
                throw new KeyNotFoundException($"Reminder with ID {id} not found");
            }

            // Add to Reminder
            var item = reminder.AddItem(description, url);

            // Save changes
            await _repository.UpdateAsync(reminder);

            // Return new Item
            return _mapper.Map<ReminderItemDTO>(item);
        }


        public async Task<ReminderTaskDTO> AddTask(int id, string description, bool isCompleted, string? url)
        {
            // Check if we have associated Reminder
            var reminder = await _repository.GetByIdAsync(id);
            if (reminder == null)
            {
                throw new KeyNotFoundException($"Reminder with ID {id} not found");
            }

            // Add to Reminder
            var task = reminder.AddTask(description, isCompleted, url);

            // Save changes
            await _repository.UpdateAsync(reminder);

            // Return new Task
            return _mapper.Map<ReminderTaskDTO>(task);
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
                return modelDTOs.OrderBy(x => x.IsCompleted).ThenByDescending(x => x.PriorityLevel).ThenByDescending(x => x.Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
