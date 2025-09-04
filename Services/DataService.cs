using Newtonsoft.Json;
using MunicipalApp.Models;

namespace MunicipalApp.Services
{
    public class DataService
    {
        private readonly string _dataFilePath;

        public DataService()
        {
            _dataFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "issues.json");
            EnsureDataDirectoryExists();
        }

        private void EnsureDataDirectoryExists()
        {
            var directory = Path.GetDirectoryName(_dataFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory!);
            }
        }

        public List<Issue> LoadIssues()
        {
            try
            {
                if (!File.Exists(_dataFilePath))
                {
                    return new List<Issue>();
                }

                var json = File.ReadAllText(_dataFilePath);
                return JsonConvert.DeserializeObject<List<Issue>>(json) ?? new List<Issue>();
            }
            catch
            {
                return new List<Issue>();
            }
        }

        public void SaveIssues(List<Issue> issues)
        {
            try
            {
                var json = JsonConvert.SerializeObject(issues, Formatting.Indented);
                File.WriteAllText(_dataFilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AddIssue(Issue issue)
        {
            var issues = LoadIssues();
            issues.Add(issue);
            SaveIssues(issues);
        }
    }
}