using System;

namespace Sidequest_municiple_app
{
    public class LocalEvent
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public DateTime EventDate { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }

        public LocalEvent(string title, string category, DateTime eventDate, string description, int priority)
        {
            Title = title;
            Category = category;
            EventDate = eventDate;
            Description = description;
            Priority = priority;
        }
    }
}
