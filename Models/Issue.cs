using System;

namespace Sidequest_municiple_app
{
    public class Issue
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string AttachmentPath { get; set; }
        public DateTime ReportDate { get; set; }
        public ServiceRequestStatus Status { get; set; }
        public ServiceRequestPriority Priority { get; set; }

        public Issue()
        {
            UniqueId = Guid.NewGuid().ToString();
            ReportDate = DateTime.Now;
            Status = ServiceRequestStatus.Pending;
            Priority = ServiceRequestPriority.Medium;
        }

        public Issue(string location, string category, string description, string attachmentPath = "")
        {
            UniqueId = Guid.NewGuid().ToString();
            Location = location;
            Category = category;
            Description = description;
            AttachmentPath = attachmentPath;
            ReportDate = DateTime.Now;
            Status = ServiceRequestStatus.Pending;
            Priority = ServiceRequestPriority.Medium;
        }
    }
}