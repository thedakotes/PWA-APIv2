using PWAApi.ApiService.Models;

namespace API.Models
{
    public class Event : BaseEntity, IUserAssociated
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string? Color { get; set; } = string.Empty; //hex string of the color of the event
        public required Guid UserID { get; set; }
    }
}