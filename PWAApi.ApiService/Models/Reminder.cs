using PWAApi.ApiService.Enums;

namespace PWAApi.ApiService.Models
{
    public class Reminder : BaseEntity, IUserAssociated
    {
        public string? Description { get; set; }
        public string? Notes { get; set; }
        public PriorityLevelEnum PriorityLevel { get; set; } = PriorityLevelEnum.None;
        private bool _isCompleted = false;
        public bool IsCompleted 
        { 
            get => _isCompleted;
            set
            {
                if (!_isCompleted && value)
                {
                    CompletedOn = DateTime.UtcNow;
                }
                else if (_isCompleted && !value)
                {
                    CompletedOn = null;
                }

                _isCompleted = value;
            }
        }
        public DateTimeOffset? CompletedOn { get; set; }
        public required Guid UserID { get; set; }
    }
}
