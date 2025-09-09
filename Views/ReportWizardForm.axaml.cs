using Avalonia.Controls;
using Avalonia.Interactivity;
using MunicipalApp.ViewModels;
using System;

namespace MunicipalApp.Views
{
    public partial class ReportWizardForm : UserControl
    {
        private ReportWizardViewModel _viewModel;

        public ReportWizardForm()
        {
            InitializeComponent();
            _viewModel = new ReportWizardViewModel();
            DataContext = _viewModel;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.NavigateBack();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.NavigateNext();
        }

        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.OpenAdminPage();
        }
    }
}