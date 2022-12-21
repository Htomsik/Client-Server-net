using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace AvaloniaUIClient.Converters;

internal sealed class HalfSizeConverter : IValueConverter
{
    
    public static readonly HalfSizeConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double && !double.IsNaN((double)value))
            return (double)value / 2;
        
        return value;
    }


    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
  
}