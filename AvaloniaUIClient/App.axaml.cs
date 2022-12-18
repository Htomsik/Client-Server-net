using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaUIClient.IOC;
using AvaloniaUIClient.Views;
using Core.Infrastructure.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AvaloniaUIClient;

public partial class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);
    
    private static IHost? _host;
    
    public static IHost? Host => _host ??= HostCreator.CreateHost(IocWorker.RegistredServies());
    
    public static IServiceProvider Services => Host.Services;

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        { 
            desktop.MainWindow = Services.GetRequiredService<MainWindow>();
            
            Host.Start();
        }

        base.OnFrameworkInitializationCompleted();
    }
}