namespace API.Models
{
    public class Event : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string? Color { get; set; } = string.Empty; //hex string of the color of the event
    }
}