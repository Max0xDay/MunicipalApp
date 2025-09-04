namespace MunicipalApp.Models
{
    public class Issue
    {
        public string Id { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> AttachedFiles { get; set; }
        public DateTime DateReported { get; set; }
        public string Status { get; set; } = string.Empty;

        public Issue()
        {
            Id = Guid.NewGuid().ToString();
            AttachedFiles = new List<string>();
            DateReported = DateTime.Now;
            Status = "Submitted";
        }
    }
}