using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using MunicipalApp.Models;
using MunicipalApp.Services;

namespace MunicipalApp.ViewModels;

public class ReportIssuesViewModel : ViewModelBase
{
    private readonly DataService _dataService;
    private string _location = string.Empty;
    private string _selectedCategory = string.Empty;
    private string _description = string.Empty;
    private int _progressValue;
    private string _progressText = "0 of 4 fields completed";
    private string _encouragementText = "Let's get started! Please fill in your issue details.";
    private bool _canSubmit;

    public ReportIssuesViewModel()
    {
        _dataService = new DataService();
        AttachedFiles = new ObservableCollection<string>();
        Categories = new ObservableCollection<string>
        {
            "Sanitation",
            "Roads", 
            "Utilities",
            "Water",
            "Electricity"
        };

        AttachFilesCommand = ReactiveCommand.Create(AttachFiles);
        RemoveFileCommand = ReactiveCommand.Create<string>(RemoveFile);
        SubmitCommand = ReactiveCommand.Create(Submit, this.WhenAnyValue(x => x.CanSubmit));

        this.WhenAnyValue(x => x.Location, x => x.SelectedCategory, x => x.Description, x => x.AttachedFiles.Count)
            .Subscribe(_ => UpdateProgress());
    }

    public string Title => "Report an Issue";
    public string Location
    {
        get => _location;
        set => this.RaiseAndSetIfChanged(ref _location, value);
    }

    public string SelectedCategory
    {
        get => _selectedCategory;
        set => this.RaiseAndSetIfChanged(ref _selectedCategory, value);
    }

    public string Description
    {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }

    public int ProgressValue
    {
        get => _progressValue;
        set => this.RaiseAndSetIfChanged(ref _progressValue, value);
    }

    public string ProgressText
    {
        get => _progressText;
        set => this.RaiseAndSetIfChanged(ref _progressText, value);
    }

    public string EncouragementText
    {
        get => _encouragementText;
        set => this.RaiseAndSetIfChanged(ref _encouragementText, value);
    }

    public bool CanSubmit
    {
        get => _canSubmit;
        set => this.RaiseAndSetIfChanged(ref _canSubmit, value);
    }

    public ObservableCollection<string> Categories { get; }
    public ObservableCollection<string> AttachedFiles { get; }

    public ReactiveCommand<Unit, Unit> AttachFilesCommand { get; }
    public ReactiveCommand<string, Unit> RemoveFileCommand { get; }
    public ReactiveCommand<Unit, Unit> SubmitCommand { get; }

    private void UpdateProgress()
    {
        int completedFields = 0;
        int totalFields = 4;

        if (!string.IsNullOrWhiteSpace(Location)) completedFields++;
        if (!string.IsNullOrWhiteSpace(SelectedCategory)) completedFields++;
        if (!string.IsNullOrWhiteSpace(Description)) completedFields++;
        if (AttachedFiles.Count > 0) completedFields++;

        int percentage = (int)((double)completedFields / totalFields * 100);
        ProgressValue = percentage;
        ProgressText = $"{completedFields} of {totalFields} fields completed";

        EncouragementText = percentage switch
        {
            0 => "Let's get started! Please fill in your issue details.",
            < 50 => "Great start! Keep going.",
            < 75 => "You're making excellent progress!",
            < 100 => "Almost done! Just a few more details.",
            _ => "Perfect! All fields completed. Ready to submit!"
        };

        CanSubmit = percentage == 100;
    }

    private void AttachFiles()
    {
        // This will be handled by the View using Avalonia's file dialogs
    }

    private void RemoveFile(string fileName)
    {
        AttachedFiles.Remove(fileName);
    }

    private void Submit()
    {
        try
        {
            var issue = new Issue
            {
                Location = Location.Trim(),
                Category = SelectedCategory,
                Description = Description.Trim(),
                AttachedFiles = new List<string>(AttachedFiles)
            };

            _dataService.AddIssue(issue);
            
            // Success handling will be done in the View
        }
        catch (Exception)
        {
            // Error handling will be done in the View
        }
    }
}