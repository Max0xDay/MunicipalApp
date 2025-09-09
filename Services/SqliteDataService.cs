using Microsoft.Data.Sqlite;
using MunicipalApp.Models;

namespace MunicipalApp.Services
{
    public class SqliteDataService
    {
        private readonly string _dbPath;

        public SqliteDataService()
        {
            // Stable per-user location instead of volatile build output folder
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var targetDir = Path.Combine(appData, "MunicipalApp");
            Directory.CreateDirectory(targetDir);
            _dbPath = Path.Combine(targetDir, "municipal.db");

            // Migrate old DB (previous location inside build output) if it exists and new one not yet created
            try
            {
                var oldPath = Path.Combine(AppContext.BaseDirectory, "Data", "municipal.db");
                if (!File.Exists(_dbPath) && File.Exists(oldPath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(_dbPath)!);
                    File.Copy(oldPath, _dbPath, overwrite: true);
                }
            }
            catch { /* ignore migration errors */ }

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
            using var tx = conn.BeginTransaction();
            try
            {
                var cmd = conn.CreateCommand();
                cmd.Transaction = tx;
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
                tx.Commit();

                // Quick verification (optional lightweight existence check)
                var verify = conn.CreateCommand();
                verify.CommandText = "SELECT 1 FROM Issues WHERE Id=$id LIMIT 1";
                verify.Parameters.AddWithValue("$id", issue.Id);
                var ok = verify.ExecuteScalar();
                if (ok == null)
                {
                    Console.WriteLine($"[SQLite] Insert verification failed for issue {issue.Id}");
                }
            }
            catch (Exception ex)
            {
                try { tx.Rollback(); } catch { }
                Console.WriteLine($"[SQLite] Insert failed: {ex.Message}");
                throw;
            }
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
