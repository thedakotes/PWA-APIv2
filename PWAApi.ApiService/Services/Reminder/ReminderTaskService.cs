using AutoMapper;
using PWAApi.ApiService.DataTransferObjects.Reminder;
using PWAApi.ApiService.Models.Events.Reminder;
using PWAApi.ApiService.Repositories.Event;

namespace PWAApi.ApiService.Services
{
    public class ReminderTaskService : EntityService<ReminderTask, ReminderTaskDTO, CreateReminderTaskDTO, IReminderTaskRepository>, IReminderTaskService
    {
        public ReminderTaskService(IMapper mapper, IReminderTaskRepository repository)
            : base(mapper, repository)
        {
        }

        public async Task<ReminderTaskDTO> Complete(int id)
        {
            try
            {
                var reminderTask = await _repository.GetByIdAsync(id);
                if (reminderTask == null)
                {
                    throw new Exception("Task not found");
                }

                reminderTask.IsCompleted = true;

                await _repository.UpdateAsync(reminderTask);
                return _mapper.Map<ReminderTaskDTO>(reminderTask);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
