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
                var issues = await _dataService.LoadIssuesAsync();
                _issues.Clear();
                foreach (var issue in issues)
                {
                    _issues.Add(issue);
                }
                FilterIssues();
            }
            catch (Exception ex)
            {
                // Handle loading errors
                Console.WriteLine($"Error loading issues: {ex.Message}");
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
            // Placeholder for export functionality
            // In a real implementation, this would export to CSV/Excel
            try
            {
                var exportData = _filteredIssues.Select(i => new
                {
                    i.Id,
                    i.DateReported,
                    i.Location,
                    i.Category,
                    i.Description,
                    i.Status,
                    PhotoCount = i.AttachedFiles.Count
                }).ToList();

                // Export logic would go here
                Console.WriteLine($"Exporting {exportData.Count} records");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting data: {ex.Message}");
            }
        }

        private void UpdateStatus()
        {
            // Placeholder for status update functionality
            if (SelectedIssue == null) return;

            // In a real implementation, this would show a dialog to update status
            Console.WriteLine($"Updating status for issue {SelectedIssue.Id}");
        }

        private void ViewDetails()
        {
            // Placeholder for view details functionality
            if (SelectedIssue == null) return;

            // In a real implementation, this would show a detailed view
            Console.WriteLine($"Viewing details for issue {SelectedIssue.Id}");
        }

        private void CloseAdmin()
        {
            // This would be handled by the parent window/control
            Console.WriteLine("Closing admin dashboard");
        }

        public ReactiveCommand<Unit, Unit> RefreshCommand => _refreshCommand;
        public ReactiveCommand<Unit, Unit> ExportCommand => _exportCommand;
        public ReactiveCommand<Unit, Unit> UpdateStatusCommand => _updateStatusCommand;
        public ReactiveCommand<Unit, Unit> ViewDetailsCommand => _viewDetailsCommand;
        public ReactiveCommand<Unit, Unit> CloseCommand => _closeCommand;
    }
}