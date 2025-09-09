using Avalonia;
using Avalonia.Controls;
using MunicipalApp.Models;
using MunicipalApp.Services;
using MunicipalApp.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;

namespace MunicipalApp.ViewModels
{
    public class ReportWizardViewModel : ViewModelBase
    {
        private UserControl _currentPage;
        private int _currentStep;
        private Issue _currentIssue;
        private ReactiveCommand<Unit, Unit> _backCommand;
        private ReactiveCommand<Unit, Unit> _nextCommand;
        private ReactiveCommand<Unit, Unit> _adminCommand;

        public ReportWizardViewModel()
        {
            _currentIssue = new Issue();
            _currentStep = 0;
            
            InitializePages();
            InitializeCommands();
            
            CurrentPage = _pages[0];
            UpdateProgress();
        }

        private List<UserControl> _pages;
        private WelcomePageViewModel _welcomePageVm;
        private LocationPageViewModel _locationPageVm;
        private CategoryPageViewModel _categoryPageVm;
        private PhotoPageViewModel _photoPageVm;
        private ConfirmationPageViewModel _confirmationPageVm;

        private void InitializePages()
        {
            _welcomePageVm = new WelcomePageViewModel();
            _locationPageVm = new LocationPageViewModel();
            _categoryPageVm = new CategoryPageViewModel();
            _photoPageVm = new PhotoPageViewModel();
            _confirmationPageVm = new ConfirmationPageViewModel();

            _pages = new List<UserControl>
            {
                new WelcomePage { DataContext = _welcomePageVm },
                new LocationPage { DataContext = _locationPageVm },
                new CategoryPage { DataContext = _categoryPageVm },
                new PhotoPage { DataContext = _photoPageVm },
                new ConfirmationPage { DataContext = _confirmationPageVm }
            };
        }

        private void InitializeCommands()
        {
            _backCommand = ReactiveCommand.Create(NavigateBack);
            _nextCommand = ReactiveCommand.Create(NavigateNext);
            _adminCommand = ReactiveCommand.Create(OpenAdminPage);
        }

        public UserControl CurrentPage
        {
            get => _currentPage;
            set => this.RaiseAndSetIfChanged(ref _currentPage, value);
        }

        public int CurrentStep
        {
            get => _currentStep;
            set => this.RaiseAndSetIfChanged(ref _currentStep, value);
        }

        public double ProgressPercentage => (CurrentStep + 1) * 20.0;
        public string ProgressText => $"Step {CurrentStep + 1} of 5";
        public string NextButtonText => CurrentStep == 4 ? "Finish" : "Next";
        public bool CanGoBack => CurrentStep > 0;
        public bool CanGoNext => CanProceedToNext();

        private bool CanProceedToNext()
        {
            return CurrentStep switch
            {
                0 => true, // Welcome page
                1 => !string.IsNullOrWhiteSpace(_locationPageVm.Location),
                2 => !string.IsNullOrWhiteSpace(_categoryPageVm.SelectedCategory) && 
                     !string.IsNullOrWhiteSpace(_categoryPageVm.Description),
                3 => _photoPageVm.UploadedFiles.Count > 0,
                4 => true, // Confirmation page
                _ => false
            };
        }

        public void NavigateBack()
        {
            if (CurrentStep > 0)
            {
                CurrentStep--;
                CurrentPage = _pages[CurrentStep];
                UpdateProgress();
            }
        }

        public void NavigateNext()
        {
            if (CurrentStep < 4)
            {
                SaveCurrentStepData();
                CurrentStep++;
                CurrentPage = _pages[CurrentStep];
                UpdateProgress();
                
                if (CurrentStep == 4)
                {
                    PrepareConfirmationPage();
                }
            }
            else
            {
                FinishWizard();
            }
        }

        private void SaveCurrentStepData()
        {
            switch (CurrentStep)
            {
                case 1:
                    _currentIssue.Location = _locationPageVm.Location;
                    break;
                case 2:
                    _currentIssue.Category = _categoryPageVm.SelectedCategory;
                    _currentIssue.Description = _categoryPageVm.Description;
                    break;
                case 3:
                    _currentIssue.AttachedFiles = _photoPageVm.UploadedFiles.Select(p => p.FilePath).ToList();
                    break;
            }
        }

        private void PrepareConfirmationPage()
        {
            _confirmationPageVm.ReportNumber = _currentIssue.Id;
            _confirmationPageVm.Location = _currentIssue.Location;
            _confirmationPageVm.Category = _currentIssue.Category;
            _confirmationPageVm.Description = _currentIssue.Description;
            _confirmationPageVm.PhotoCount = _currentIssue.AttachedFiles.Count;
        }

        private void FinishWizard()
        {
            var dataService = new DataService();
            dataService.SaveIssue(_currentIssue);
            
            // Reset wizard for new report
            _currentIssue = new Issue();
            CurrentStep = 0;
            CurrentPage = _pages[0];
            UpdateProgress();
            
            // Clear form data
            _locationPageVm.Clear();
            _categoryPageVm.Clear();
            _photoPageVm.Clear();
        }

        private void UpdateProgress()
        {
            this.RaisePropertyChanged(nameof(ProgressPercentage));
            this.RaisePropertyChanged(nameof(ProgressText));
            this.RaisePropertyChanged(nameof(NextButtonText));
            this.RaisePropertyChanged(nameof(CanGoBack));
            this.RaisePropertyChanged(nameof(CanGoNext));
        }

        public void OpenAdminPage()
        {
            // Navigate to admin page - this will be handled by the parent window
            // The command binding in XAML will handle the navigation
        }

        public ReactiveCommand<Unit, Unit> BackCommand => _backCommand;
        public ReactiveCommand<Unit, Unit> NextCommand => _nextCommand;
        public ReactiveCommand<Unit, Unit> AdminCommand => _adminCommand;
    }
}