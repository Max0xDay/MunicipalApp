namespace MunicipalApp.Models
{
    public class Event
    {
        public string Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Organizer { get; set; } = string.Empty;
        public int Priority { get; set; }
        public List<string> Tags { get; set; }
        public DateTime CreatedDate { get; set; }

        public Event()
        {
            Id = Guid.NewGuid().ToString();
            Tags = new List<string>();
            CreatedDate = DateTime.Now;
            Priority = 1;
        }
    }
}