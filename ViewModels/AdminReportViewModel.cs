using MunicipalApp.Models;
using MunicipalApp.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace MunicipalApp.ViewModels
{
    public class AdminReportViewModel : ViewModelBase
    {
    private readonly DataService _dataService;
        private ObservableCollection<Issue> _issues;
        private ObservableCollection<Issue> _filteredIssues;
        private string _searchText = string.Empty;
        private string _selectedCategory = "All";
        private string _selectedStatus = "All";
    private Issue _selectedIssue = null!;
    private List<string> _categories = null!;
    private List<string> _statuses = null!;
    private ReactiveCommand<Unit, Unit> _refreshCommand = null!;
    private ReactiveCommand<Unit, Unit> _exportCommand = null!;
    private ReactiveCommand<Unit, Unit> _updateStatusCommand = null!;
    private ReactiveCommand<Unit, Unit> _viewDetailsCommand = null!;
    private ReactiveCommand<Unit, Unit> _closeCommand = null!;

        public AdminReportViewModel()
        {
            _dataService = new DataService();
            _issues = new ObservableCollection<Issue>();
            _filteredIssues = new ObservableCollection<Issue>();
            
            InitializeCollections();
            InitializeCommands();
            _ = LoadIssues(); // Fire and forget
            
            ReportWizardViewModel.IssueCreated -= OnIssueCreated;
            ReportWizardViewModel.IssueCreated += OnIssueCreated;
        }

        private void InitializeCollections()
        {
            _categories = new List<string> { "All", "Sanitation", "Roads", "Utilities", "Water", "Electricity" };
            _statuses = new List<string> { "All", "Submitted", "In Progress", "Resolved", "Closed" };
        }

        private void InitializeCommands()
        {
            _refreshCommand = ReactiveCommand.CreateFromTask(LoadIssues);
            _exportCommand = ReactiveCommand.Create(ExportData);
            _updateStatusCommand = ReactiveCommand.Create(UpdateStatus);
            _viewDetailsCommand = ReactiveCommand.Create(ViewDetails);
            _closeCommand = ReactiveCommand.Create(CloseAdmin);
        }

        public ObservableCollection<Issue> Issues => _issues;
        public ObservableCollection<Issue> FilteredIssues => _filteredIssues;

        public string SearchText
        {
            get => _searchText;
            set
            {
                this.RaiseAndSetIfChanged(ref _searchText, value);
                FilterIssues();
            }
        }

        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedCategory, value);
                FilterIssues();
            }
        }

        public string SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedStatus, value);
                FilterIssues();
            }
        }

        public Issue SelectedIssue
        {
            get => _selectedIssue;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedIssue, value);
                this.RaisePropertyChanged(nameof(HasSelectedIssue));
            }
        }

        public bool HasSelectedIssue => SelectedIssue != null;
    public List<string> Categories => _categories;
    public List<string> Statuses => _statuses;
        public string StatusText => $"Showing {FilteredIssues.Count} of {Issues.Count} issues";

        private async Task LoadIssues()
        {
            try
            {
                var issues = await Task.Run(() => _dataService.LoadIssues());
                _issues.Clear();
                foreach (var issue in issues)
                {
                    _issues.Add(issue);
                }
                FilterIssues();
                
                System.Diagnostics.Debug.WriteLine($"=== LOADED ISSUES ===");
                System.Diagnostics.Debug.WriteLine($"Total issues loaded: {issues.Count}");
                foreach (var issue in issues)
                {
                    System.Diagnostics.Debug.WriteLine($"- {issue.Id}: {issue.Location} ({issue.Category})");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading issues: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Error loading issues: {ex.Message}");
            }
        }

        private void FilterIssues()
        {
            var filtered = _issues.AsEnumerable();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var searchLower = SearchText.ToLower();
                filtered = filtered.Where(i => 
                    i.Location.ToLower().Contains(searchLower) ||
                    i.Category.ToLower().Contains(searchLower) ||
                    i.Id.ToLower().Contains(searchLower) ||
                    i.Description.ToLower().Contains(searchLower));
            }

            // Apply category filter
            if (SelectedCategory != "All")
            {
                filtered = filtered.Where(i => i.Category == SelectedCategory);
            }

            // Apply status filter
            if (SelectedStatus != "All")
            {
                filtered = filtered.Where(i => i.Status == SelectedStatus);
            }

            _filteredIssues.Clear();
            foreach (var issue in filtered.OrderByDescending(i => i.DateReported))
            {
                _filteredIssues.Add(issue);
            }

            this.RaisePropertyChanged(nameof(StatusText));
        }

        private void ExportData()
        {
            try
            {
                if (_filteredIssues.Count == 0)
                {
                    StatusMessageRequested?.Invoke("No records to export.");
                    return;
                }

                var lines = new List<string>();
                lines.Add("Id,DateReported,Location,Category,Description,Status,PhotoCount");
                foreach (var i in _filteredIssues)
                {
                    // Basic CSV escaping for commas and quotes
                    string Esc(string? s) => string.IsNullOrEmpty(s) ? "" : "\"" + s.Replace("\"", "\"\"") + "\"";
                    lines.Add(string.Join(',', new[]
                    {
                        Esc(i.Id),
                        Esc(i.DateReported.ToString("o")),
                        Esc(i.Location),
                        Esc(i.Category),
                        Esc(i.Description),
                        Esc(i.Status),
                        i.AttachedFiles.Count.ToString()
                    }));
                }

                var exportDir = Path.Combine(AppContext.BaseDirectory, "Exports");
                Directory.CreateDirectory(exportDir);
                var fileName = $"issues_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv";
                var path = Path.Combine(exportDir, fileName);
                File.WriteAllLines(path, lines);
                StatusMessageRequested?.Invoke($"Exported {_filteredIssues.Count} records to {fileName}");
            }
            catch (Exception ex)
            {
                StatusMessageRequested?.Invoke($"Export failed: {ex.Message}");
            }
        }

        private void UpdateStatus()
        {
            if (SelectedIssue == null) return;
            
            var order = new[] { "Submitted", "In Progress", "Resolved", "Closed" };
            var currentIndex = Array.IndexOf(order, SelectedIssue.Status);
            var nextStatus = order[(currentIndex + 1) % order.Length];
            try
            {
                var allIssues = _dataService.LoadIssues();
                var issueToUpdate = allIssues.FirstOrDefault(i => i.Id == SelectedIssue.Id);
                if (issueToUpdate != null)
                {
                    issueToUpdate.Status = nextStatus;
                    _dataService.SaveIssues(allIssues);
                    SelectedIssue.Status = nextStatus;
                    this.RaisePropertyChanged(nameof(SelectedIssue));
                    FilterIssues();
                    StatusMessageRequested?.Invoke($"Status updated to {nextStatus}");
                }
            }
            catch (Exception ex)
            {
                StatusMessageRequested?.Invoke($"Status update failed: {ex.Message}");
            }
        }

        private void ViewDetails()
        {
            if (SelectedIssue == null) return;
            IssueDetailsRequested?.Invoke(SelectedIssue);
        }

        private void CloseAdmin()
        {
            CloseRequested?.Invoke();
        }

        private void OnIssueCreated(Issue issue)
        {
            // Avoid duplicates
            if (!_issues.Any(i => i.Id == issue.Id))
            {
                _issues.Add(issue);
                FilterIssues();
                StatusMessageRequested?.Invoke($"New issue added: {issue.Id}");
            }
        }

        // Events for the host window/view to react (show dialogs, navigate, toast, etc.)
        public static event Action<Issue>? IssueDetailsRequested;
        public static event Action<string>? StatusMessageRequested;
        public static event Action? CloseRequested;

        public ReactiveCommand<Unit, Unit> RefreshCommand => _refreshCommand;
        public ReactiveCommand<Unit, Unit> ExportCommand => _exportCommand;
        public ReactiveCommand<Unit, Unit> UpdateStatusCommand => _updateStatusCommand;
        public ReactiveCommand<Unit, Unit> ViewDetailsCommand => _viewDetailsCommand;
        public ReactiveCommand<Unit, Unit> CloseCommand => _closeCommand;
    }
}