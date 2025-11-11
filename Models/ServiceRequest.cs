using System;
using System.Collections.Generic;

namespace Sidequest_municiple_app {
    public enum ServiceRequestStatus {
        Pending,
        InProgress,
        Completed,
        Rejected
    }

    public enum ServiceRequestPriority {
        Low = 1,
        Medium = 2,
        High = 3,
        Urgent = 4
    }

    public class ServiceRequest {
        public string UniqueID { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string AttachmentPath { get; set; }
        public DateTime DateSubmitted { get; set; }
        public ServiceRequestStatus Status { get; set; }
        public ServiceRequestPriority Priority { get; set; }
        public List<string> RelatedRequestIDs { get; set; }

        public ServiceRequest() {
            UniqueID = Guid.NewGuid().ToString();
            DateSubmitted = DateTime.Now;
            Status = ServiceRequestStatus.Pending;
            Priority = ServiceRequestPriority.Medium;
            RelatedRequestIDs = new List<string>();
        }

        public ServiceRequest(string location, string category, string description, string attachmentPath = "", string uniqueId = null) {
            UniqueID = string.IsNullOrWhiteSpace(uniqueId) ? Guid.NewGuid().ToString() : uniqueId;
            Location = location;
            Category = category;
            Description = description;
            AttachmentPath = attachmentPath;
            DateSubmitted = DateTime.Now;
            Status = ServiceRequestStatus.Pending;
            Priority = ServiceRequestPriority.Medium;
            RelatedRequestIDs = new List<string>();
        }

        public ServiceRequest(Issue issue) {
            UniqueID = string.IsNullOrWhiteSpace(issue.UniqueId) ? Guid.NewGuid().ToString() : issue.UniqueId;
            Location = issue.Location;
            Category = issue.Category;
            Description = issue.Description;
            AttachmentPath = issue.AttachmentPath;
            DateSubmitted = issue.ReportDate;
            Status = issue.Status;
            Priority = issue.Priority;
            RelatedRequestIDs = new List<string>();
            if (string.IsNullOrWhiteSpace(issue.UniqueId))
                issue.UniqueId = UniqueID;
        }

        public string GetStatusString() {
            return Status.ToString();
        }

        public string GetPriorityString() {
            return Priority.ToString();
        }
    }
}
