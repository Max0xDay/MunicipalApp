using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using MunicipalApp.ViewModels;
using ReactiveUI;
using System.Reactive;

namespace MunicipalApp.Views;

public partial class ReportIssuesView : UserControl
{
    public ReportIssuesView()
    {
        InitializeComponent();
        this.Loaded += OnLoaded;
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ReportIssuesViewModel vm)
        {
            vm.AttachFilesCommand.Subscribe(async _ => await AttachFiles());
            vm.SubmitCommand.Subscribe(_ => ShowSubmissionResult());
        }
    }

    private async Task AttachFiles()
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null) return;

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Attach Files",
            AllowMultiple = true,
            FileTypeFilter = new[] 
            {
                FilePickerFileTypes.All,
                FilePickerFileTypes.ImageAll,
                FilePickerFileTypes.Pdf
            }
        });

        if (DataContext is ReportIssuesViewModel vm)
        {
            foreach (var file in files)
            {
                var fileName = file.Name;
                if (!vm.AttachedFiles.Contains(fileName))
                {
                    vm.AttachedFiles.Add(fileName);
                }
            }
        }
    }

    private async void ShowSubmissionResult()
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null) return;

        await ShowMessageBox(topLevel, "Success", 
            "Thank you! Your issue has been successfully submitted.\n\n" +
            "Your issue has been recorded and assigned a reference ID.\n" +
            "Status: Submitted");
    }

    private static async Task ShowMessageBox(TopLevel topLevel, string title, string message)
    {
        var dialog = new Window
        {
            Title = title,
            Width = 400,
            Height = 200,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        var okButton = new Button 
        { 
            Content = "OK",
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
        };
        
        okButton.Click += (_, _) => dialog.Close();

        dialog.Content = new StackPanel
        {
            Margin = new Avalonia.Thickness(20),
            Spacing = 15,
            Children =
            {
                new TextBlock 
                { 
                    Text = message, 
                    TextWrapping = Avalonia.Media.TextWrapping.Wrap 
                },
                okButton
            }
        };

        await dialog.ShowDialog((Window)topLevel);
    }
}