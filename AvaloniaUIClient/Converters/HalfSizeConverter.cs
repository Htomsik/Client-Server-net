using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace AvaloniaUIClient.Converters;

internal sealed class HalfSizeConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is double convValue &&
        !double.IsNaN(convValue) ? 
            convValue / 2 :
            value;
    
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}