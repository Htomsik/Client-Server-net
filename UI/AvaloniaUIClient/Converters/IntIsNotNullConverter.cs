using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace AvaloniaUIClient.Converters;

public class IntIsNotNullConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is int val && val != 0;
    
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value;
}