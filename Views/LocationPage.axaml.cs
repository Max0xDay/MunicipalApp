using Avalonia.Controls;
using Avalonia.Input;
using MunicipalApp.ViewModels;
using System;

namespace MunicipalApp.Views
{
    public partial class LocationPage : UserControl
    {
        private LocationPageViewModel _viewModel;

        public LocationPage()
        {
            InitializeComponent();
            _viewModel = (LocationPageViewModel)DataContext;
        }

        private void LocationTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            var textBox = (TextBox)sender;
            _viewModel?.LocationTextChanged(textBox.Text);
        }
    }
}