using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sidequest_municiple_app
{
    public class DatabaseHelper
    {
        private readonly string dataFilePath;
        private static int nextId = 1;

        public DatabaseHelper()
        {
            dataFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MunicipalApp.txt");
            InitializeDataFile();
        }

        private void InitializeDataFile()
        {
            if (!File.Exists(dataFilePath))
            {
                SaveIssuesList(new List<Issue>());
            }
            else
            {
                var existingIssues = LoadIssuesList();
                if (existingIssues.Any())
                {
                    nextId = existingIssues.Max(i => i.Id) + 1;
                }
            }
        }

        public int SaveIssue(Issue issue)
        {
            try
            {
                var issues = LoadIssuesList();
                issue.Id = nextId++;
                issues.Add(issue);
                SaveIssuesList(issues);
                return issue.Id;
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving issue: " + ex.Message);
            }
        }

        public List<Issue> GetAllIssues()
        {
            try
            {
                return LoadIssuesList().OrderByDescending(i => i.ReportDate).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading issues: " + ex.Message);
            }
        }

        private List<Issue> LoadIssuesList()
        {
            try
            {
                if (!File.Exists(dataFilePath))
                    return new List<Issue>();

                string[] lines = File.ReadAllLines(dataFilePath);
                List<Issue> issues = new List<Issue>();

                foreach (string line in lines)
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    
                    string[] parts = line.Split('|');
                    if (parts.Length >= 6)
                    {
                        Issue issue = new Issue
                        {
                            Id = int.Parse(parts[0]),
                            Location = parts[1],
                            Category = parts[2],
                            Description = parts[3],
                            AttachmentPath = parts[4],
                            ReportDate = DateTime.Parse(parts[5])
                        };
                        issues.Add(issue);
                    }
                }

                return issues;
            }
            catch
            {
                return new List<Issue>();
            }
        }

        private void SaveIssuesList(List<Issue> issues)
        {
            List<string> lines = new List<string>();
            foreach (Issue issue in issues)
            {
                string line = $"{issue.Id}|{issue.Location}|{issue.Category}|{issue.Description}|{issue.AttachmentPath ?? ""}|{issue.ReportDate:yyyy-MM-dd HH:mm:ss}";
                lines.Add(line);
            }
            File.WriteAllLines(dataFilePath, lines);
        }
    }
}