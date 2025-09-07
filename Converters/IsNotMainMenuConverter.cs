using System;
using System.Globalization;
using Avalonia.Data.Converters;
using MunicipalApp.ViewModels;

namespace MunicipalApp.Converters;

public class IsNotMainMenuConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is not MainMenuViewModel;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}