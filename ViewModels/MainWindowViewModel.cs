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
    _currentView = new ReportWizardViewModel();
        
        ReportIssuesCommand = ReactiveCommand.Create(() => 
        {
            CurrentView = new ReportWizardViewModel();
            HookWizardAdmin();
            // After returning to wizard, ensure admin events still hooked (they are static)
            HookAdminEvents();
        });
        
        ServiceStatusCommand = ReactiveCommand.Create(() => 
        {
            // MessageBox equivalent will be handled in the View
        });
        
        AdminCommand = ReactiveCommand.Create(() =>
        {
            CurrentView = new AdminReportViewModel();
            HookAdminEvents();
        });

        BackToMainCommand = ReactiveCommand.Create(() =>
        {
            CurrentView = new ReportWizardViewModel();
            HookWizardAdmin();
            HookAdminEvents();
        });

        HookWizardAdmin();
        HookAdminEvents();
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

    private void HookWizardAdmin()
    {
        ReportWizardViewModel.AdminNavigationRequested -= OnAdminNav;
        ReportWizardViewModel.AdminNavigationRequested += OnAdminNav;
    }

    private void OnAdminNav()
    {
        CurrentView = new AdminReportViewModel();
        HookAdminEvents();
    }

    private void HookAdminEvents()
    {
        // Unsubscribe first to avoid multiple handlers
        AdminReportViewModel.CloseRequested -= OnAdminClose;
        AdminReportViewModel.CloseRequested += OnAdminClose;
    }

    private void OnAdminClose()
    {
        // Navigate back to a fresh wizard (start of quiz)
        CurrentView = new ReportWizardViewModel();
        HookWizardAdmin();
        // Admin events remain hooked for future navigation
    }
}