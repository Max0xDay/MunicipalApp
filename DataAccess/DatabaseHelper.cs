using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace Sidequest_municiple_app {
    public class DatabaseHelper {
        private readonly string databasePath;
        private readonly string connectionString;

        public DatabaseHelper() {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MunicipalApp");
            Directory.CreateDirectory(folderPath);
            databasePath = Path.Combine(folderPath, "municipal.db");
            connectionString = $"Data Source={databasePath};Version=3;";
            InitializeDatabase();
        }

        public int SaveIssue(Issue issue) {
            if (issue == null) {
                throw new ArgumentNullException(nameof(issue));
            }

            if (string.IsNullOrWhiteSpace(issue.UniqueId)) {
                issue.UniqueId = Guid.NewGuid().ToString();
            }

            try {
                using (SQLiteConnection connection = CreateConnection()) {
                    connection.Open();
                    string sql = @"INSERT INTO Issues (UniqueId, Title, Location, Category, Description, AttachmentPath, ReportDate, Status, Priority)
                                   VALUES (@uniqueId, @title, @location, @category, @description, @attachmentPath, @reportDate, @status, @priority);
                                   SELECT last_insert_rowid();";
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection)) {
                        command.Parameters.AddWithValue("@uniqueId", issue.UniqueId);
                        command.Parameters.AddWithValue("@title", string.IsNullOrWhiteSpace(issue.Title) ? (object)DBNull.Value : issue.Title);
                        command.Parameters.AddWithValue("@location", issue.Location);
                        command.Parameters.AddWithValue("@category", issue.Category);
                        command.Parameters.AddWithValue("@description", issue.Description);
                        command.Parameters.AddWithValue("@attachmentPath", string.IsNullOrWhiteSpace(issue.AttachmentPath) ? (object)DBNull.Value : issue.AttachmentPath);
                        command.Parameters.AddWithValue("@reportDate", issue.ReportDate.ToString("o"));
                        command.Parameters.AddWithValue("@status", issue.Status.ToString());
                        command.Parameters.AddWithValue("@priority", (int)issue.Priority);
                        object result = command.ExecuteScalar();
                        issue.Id = Convert.ToInt32(result);
                        return issue.Id;
                    }
                }
            }
            catch (Exception ex) {
                throw new InvalidOperationException("Error saving issue: " + ex.Message, ex);
            }
        }

        public List<Issue> GetAllIssues() {
            try {
                List<Issue> issues = new List<Issue>();
                using (SQLiteConnection connection = CreateConnection()) {
                    connection.Open();
                    string sql = @"SELECT Id, UniqueId, Title, Location, Category, Description, AttachmentPath, ReportDate, Status, Priority
                                   FROM Issues
                                   ORDER BY datetime(ReportDate) DESC";
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    using (SQLiteDataReader reader = command.ExecuteReader()) {
                        while (reader.Read()) {
                            Issue issue = new Issue();
                            issue.Id = reader.GetInt32(0);
                            issue.UniqueId = reader.IsDBNull(1) ? Guid.NewGuid().ToString() : reader.GetString(1);
                            issue.Title = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                            issue.Location = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                            issue.Category = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                            issue.Description = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
                            issue.AttachmentPath = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
                            string dateValue = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
                            issue.ReportDate = ParseDate(dateValue);
                            string statusValue = reader.IsDBNull(8) ? ServiceRequestStatus.Pending.ToString() : reader.GetString(8);
                            issue.Status = ParseStatus(statusValue);
                            int priorityValue = reader.IsDBNull(9) ? (int)ServiceRequestPriority.Medium : reader.GetInt32(9);
                            issue.Priority = ParsePriority(priorityValue);
                            issues.Add(issue);
                        }
                    }
                }

                return issues;
            }
            catch (Exception ex) {
                throw new InvalidOperationException("Error loading issues: " + ex.Message, ex);
            }
        }

        public Issue GetIssueByUniqueId(string uniqueId) {
            if (string.IsNullOrWhiteSpace(uniqueId)) {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(uniqueId));
            }

            try {
                using (SQLiteConnection connection = CreateConnection()) {
                    connection.Open();
                    string sql = @"SELECT Id, UniqueId, Location, Category, Description, AttachmentPath, ReportDate, Status, Priority
                                   FROM Issues WHERE UniqueId = @uniqueId";
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection)) {
                        command.Parameters.AddWithValue("@uniqueId", uniqueId);
                        using (SQLiteDataReader reader = command.ExecuteReader()) {
                            if (!reader.Read()) {
                                return null;
                            }

                            Issue issue = new Issue();
                            issue.Id = reader.GetInt32(0);
                            issue.UniqueId = reader.IsDBNull(1) ? uniqueId : reader.GetString(1);
                            issue.Location = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                            issue.Category = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                            issue.Description = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                            issue.AttachmentPath = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
                            string dateValue = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
                            issue.ReportDate = ParseDate(dateValue);
                            string statusValue = reader.IsDBNull(7) ? ServiceRequestStatus.Pending.ToString() : reader.GetString(7);
                            issue.Status = ParseStatus(statusValue);
                            int priorityValue = reader.IsDBNull(8) ? (int)ServiceRequestPriority.Medium : reader.GetInt32(8);
                            issue.Priority = ParsePriority(priorityValue);
                            return issue;
                        }
                    }
                }
            }
            catch (Exception ex) {
                throw new InvalidOperationException("Error loading issue: " + ex.Message, ex);
            }
        }

        public void UpdateIssueStatus(string uniqueId, ServiceRequestStatus status, ServiceRequestPriority priority) {
            if (string.IsNullOrWhiteSpace(uniqueId)) {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(uniqueId));
            }

            try {
                using (SQLiteConnection connection = CreateConnection()) {
                    connection.Open();
                    string sql = @"UPDATE Issues
                                   SET Status = @status,
                                       Priority = @priority
                                   WHERE UniqueId = @uniqueId";
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection)) {
                        command.Parameters.AddWithValue("@status", status.ToString());
                        command.Parameters.AddWithValue("@priority", (int)priority);
                        command.Parameters.AddWithValue("@uniqueId", uniqueId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex) {
                throw new InvalidOperationException("Error updating issue status: " + ex.Message, ex);
            }
        }

        private void InitializeDatabase() {
            using (SQLiteConnection connection = CreateConnection()) {
                connection.Open();
                
                string createTableSql = @"CREATE TABLE IF NOT EXISTS Issues (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                UniqueId TEXT NOT NULL,
                                Title TEXT,
                                Location TEXT NOT NULL,
                                Category TEXT NOT NULL,
                                Description TEXT,
                                AttachmentPath TEXT,
                                ReportDate TEXT NOT NULL,
                                Status TEXT NOT NULL,
                                Priority INTEGER NOT NULL
                              )";
                using (SQLiteCommand command = new SQLiteCommand(createTableSql, connection)) {
                    command.ExecuteNonQuery();
                }

                string alterTableSql = @"ALTER TABLE Issues ADD COLUMN Title TEXT";
                try {
                    using (SQLiteCommand command = new SQLiteCommand(alterTableSql, connection)) {
                        command.ExecuteNonQuery();
                    }
                }
                catch (SQLiteException) {
                }
            }
        }

        private SQLiteConnection CreateConnection() {
            return new SQLiteConnection(connectionString);
        }

        private DateTime ParseDate(string value) {
            if (string.IsNullOrWhiteSpace(value)) {
                return DateTime.Now;
            }

            if (DateTime.TryParse(value, out DateTime parsed)) {
                return parsed;
            }

            return DateTime.Now;
        }

        private ServiceRequestStatus ParseStatus(string value) {
            if (Enum.TryParse(value, true, out ServiceRequestStatus status)) {
                return status;
            }

            return ServiceRequestStatus.Pending;
        }

        private ServiceRequestPriority ParsePriority(int value) {
            if (Enum.IsDefined(typeof(ServiceRequestPriority), value)) {
                return (ServiceRequestPriority)value;
            }

            return ServiceRequestPriority.Medium;
        }

        public void DeleteAllIssues() {
            try {
                using (SQLiteConnection connection = CreateConnection()) {
                    connection.Open();
                    string sql = "DELETE FROM Issues";
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection)) {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex) {
                throw new InvalidOperationException("Error deleting all issues: " + ex.Message, ex);
            }
        }
    }
}