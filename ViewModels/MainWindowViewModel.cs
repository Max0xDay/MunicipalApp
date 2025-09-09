using ReactiveUI;
using System.Reactive;
using MunicipalApp.Views;
using Avalonia.Controls;

namespace MunicipalApp.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase _currentView;

    public MainWindowViewModel()
    {
        _currentView = new MainMenuViewModel();
        
        ReportIssuesCommand = ReactiveCommand.Create(() => 
        {
            CurrentView = new ReportWizardViewModel();
        });
        
        ServiceStatusCommand = ReactiveCommand.Create(() => 
        {
            // MessageBox equivalent will be handled in the View
        });
        
        AdminCommand = ReactiveCommand.Create(() =>
        {
            CurrentView = new AdminReportViewModel();
        });

        BackToMainCommand = ReactiveCommand.Create(() =>
        {
            CurrentView = new MainMenuViewModel();
        });
    }

    public ViewModelBase CurrentView
    {
        get => _currentView;
        set => this.RaiseAndSetIfChanged(ref _currentView, value);
    }

    public ReactiveCommand<Unit, Unit> ReportIssuesCommand { get; }
    public ReactiveCommand<Unit, Unit> ServiceStatusCommand { get; }
    public ReactiveCommand<Unit, Unit> AdminCommand { get; }
    public ReactiveCommand<Unit, Unit> BackToMainCommand { get; }
}