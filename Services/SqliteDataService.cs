using Microsoft.Data.Sqlite;
using MunicipalApp.Models;

namespace MunicipalApp.Services
{
    public class SqliteDataService
    {
        private readonly string _dbPath;

        public SqliteDataService()
        {
            _dbPath = Path.Combine(AppContext.BaseDirectory, "Data", "municipal.db");
            Directory.CreateDirectory(Path.GetDirectoryName(_dbPath)!);
            Initialize();
        }

        private SqliteConnection GetConnection() => new($"Data Source={_dbPath}");

        private void Initialize()
        {
            using var conn = GetConnection();
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
CREATE TABLE IF NOT EXISTS Issues (
    Id TEXT PRIMARY KEY,
    Location TEXT NOT NULL,
    Category TEXT NOT NULL,
    Description TEXT NOT NULL,
    AttachedFiles TEXT NOT NULL,
    DateReported TEXT NOT NULL,
    Status TEXT NOT NULL
);";
            cmd.ExecuteNonQuery();
        }

        public void InsertIssue(Issue issue)
        {
            using var conn = GetConnection();
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO Issues (Id,Location,Category,Description,AttachedFiles,DateReported,Status)
VALUES ($id,$loc,$cat,$desc,$files,$date,$status);";
            cmd.Parameters.AddWithValue("$id", issue.Id);
            cmd.Parameters.AddWithValue("$loc", issue.Location);
            cmd.Parameters.AddWithValue("$cat", issue.Category);
            cmd.Parameters.AddWithValue("$desc", issue.Description);
            cmd.Parameters.AddWithValue("$files", string.Join('|', issue.AttachedFiles));
            cmd.Parameters.AddWithValue("$date", issue.DateReported.ToString("o"));
            cmd.Parameters.AddWithValue("$status", issue.Status);
            cmd.ExecuteNonQuery();
        }

        public List<Issue> GetIssues()
        {
            var list = new List<Issue>();
            using var conn = GetConnection();
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id,Location,Category,Description,AttachedFiles,DateReported,Status FROM Issues";
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var files = rdr.GetString(4).Split('|', StringSplitOptions.RemoveEmptyEntries).ToList();
                list.Add(new Issue
                {
                    Id = rdr.GetString(0),
                    Location = rdr.GetString(1),
                    Category = rdr.GetString(2),
                    Description = rdr.GetString(3),
                    AttachedFiles = files,
                    DateReported = DateTime.Parse(rdr.GetString(5), null, System.Globalization.DateTimeStyles.RoundtripKind),
                    Status = rdr.GetString(6)
                });
            }
            return list;
        }

        public void UpdateStatus(string id, string status)
        {
            using var conn = GetConnection();
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Issues SET Status=$s WHERE Id=$id";
            cmd.Parameters.AddWithValue("$s", status);
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
