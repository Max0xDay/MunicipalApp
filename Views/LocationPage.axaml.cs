using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using MunicipalApp.ViewModels;
using System;

namespace MunicipalApp.Views
{
    public partial class LocationPage : UserControl
    {
        private LocationPageViewModel? _viewModel;

        public LocationPage()
        {
            InitializeComponent();
            _viewModel = DataContext as LocationPageViewModel;
        }

        private async void LocationTextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                if (_viewModel != null)
                {
                    await _viewModel.SearchAsync();
                }
            }
        }
    }
}