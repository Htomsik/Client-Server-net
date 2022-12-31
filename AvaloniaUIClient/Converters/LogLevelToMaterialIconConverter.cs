using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Material.Icons;
using Material.Icons.Avalonia;
using Serilog.Events;

namespace AvaloniaUIClient.Converters;

public class LogLevelToMaterialIconConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        MaterialIcon icon = new MaterialIcon();

        icon.Kind = MaterialIconKind.Information;
        
        LogEventLevel level = value is LogEventLevel ? (LogEventLevel)value : LogEventLevel.Information;

        switch (level)
        {
            case (LogEventLevel.Debug):
                icon.Kind = MaterialIconKind.DevTo;
                icon.Foreground = Brushes.WhiteSmoke;
                break;
            case (LogEventLevel.Warning):
                icon.Kind  = MaterialIconKind.Alert;
                icon.Foreground = Brushes.LightGoldenrodYellow;
                break;
            case (LogEventLevel.Error):
                icon.Kind  = MaterialIconKind.AlertOctagon;
                icon.Foreground = Brushes.Red;
                break;
            case (LogEventLevel.Fatal):
                icon.Kind  = MaterialIconKind.AlertDecagram;
                icon.Foreground = Brushes.DarkRed;
                break;
        }

        return icon;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value;
}