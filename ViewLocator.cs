using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using MunicipalApp.ViewModels;

namespace MunicipalApp;

public class ViewLocator : IDataTemplate
{
    public Control Build(object? data)
    {
        if (data is null)
            return new TextBlock { Text = "No ViewModel specified" };

        var vmType = data.GetType();
        var fullName = vmType.FullName!; // e.g. MunicipalApp.ViewModels.ReportWizardViewModel
        // Normalize namespace from .ViewModels. -> .Views.
        fullName = fullName.Replace(".ViewModels.", ".Views.");

        // Candidate name patterns in order of likelihood given existing files: *Form, *View, *Page
        string baseName = fullName.EndsWith("ViewModel") ? fullName.Substring(0, fullName.Length - "ViewModel".Length) : fullName;

        var candidates = new[]
        {
            baseName + "Form",   // ReportWizardForm, AdminReportForm
            baseName + "View",   // Conventional *View
            baseName + "Page"    // Any *Page pattern
        };

        var assembly = vmType.Assembly;
        foreach (var candidate in candidates)
        {
            var type = assembly.GetType(candidate);
            if (type != null)
            {
                try
                {
                    return (Control)Activator.CreateInstance(type)!;
                }
                catch (Exception ex)
                {
                    return new TextBlock { Text = $"Error creating view {candidate}: {ex.Message}" };
                }
            }
        }

        return new TextBlock { Text = $"Not Found: {string.Join(", ", candidates)}" };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}