using Avalonia.Controls;
using AvaloniaUIClient.Infrastructure.Services.Other;
using Microsoft.Extensions.DependencyInjection;


namespace AvaloniaUIClient.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        var notifyService = App.Services.GetRequiredService<NotificationService>();
        
        notifyService.SetHostWindow(VisualRoot as TopLevel);
    }
    
}