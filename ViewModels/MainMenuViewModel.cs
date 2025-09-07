using ReactiveUI;
using System.Reactive;

namespace MunicipalApp.ViewModels;

public class MainMenuViewModel : ViewModelBase
{
    public MainMenuViewModel()
    {
        Title = "Municipal Services";
        Subtitle = "Your Gateway to City Services";
        Version = "Municipal Services App v2.0 - Part 1 & 2 Complete";
        
        ReportIssuesCommand = ReactiveCommand.Create(() => { });
        LocalEventsCommand = ReactiveCommand.Create(() => { });
        ServiceStatusCommand = ReactiveCommand.Create(() => { });
    }

    public string Title { get; }
    public string Subtitle { get; }
    public string Version { get; }
    public string ReportIssuesTitle => "Report Issues";
    public string ReportIssuesDescription => "Submit municipal issues like potholes, broken streetlights, and service requests with photo attachments.";
    public string LocalEventsTitle => "Local Events";
    public string LocalEventsDescription => "Browse community events, municipal announcements, and local activities with smart recommendations.";
    public string ServiceStatusTitle => "Service Status";
    public string ServiceStatusDescription => "Track your service requests, view status updates, and manage your submissions. (Coming in Part 3)";
    
    public ReactiveCommand<Unit, Unit> ReportIssuesCommand { get; }
    public ReactiveCommand<Unit, Unit> LocalEventsCommand { get; }
    public ReactiveCommand<Unit, Unit> ServiceStatusCommand { get; }
}