namespace Products.API.Models
{
    public class EventDto
    {
        public Guid Id { get; set; }
        public string Data { get; set; }
        public EventType EventType { get; set; }
    }
}
