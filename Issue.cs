using System;

namespace Sidequest_municiple_app
{
    public class Issue
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string AttachmentPath { get; set; }
        public DateTime ReportDate { get; set; }

        public Issue()
        {
            ReportDate = DateTime.Now;
        }

        public Issue(string location, string category, string description, string attachmentPath = "")
        {
            Location = location;
            Category = category;
            Description = description;
            AttachmentPath = attachmentPath;
            ReportDate = DateTime.Now;
        }
    }
}