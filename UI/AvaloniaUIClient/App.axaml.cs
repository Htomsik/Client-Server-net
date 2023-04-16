using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaUIClient.Infrastructure.IOC;
using AvaloniaUIClient.Views;
using Core.Infrastructure.Hosting;
using Core.Infrastructure.Services.AccountService;
using Core.Infrastructure.Services.NavigationService;
using Core.Infrastructure.VMD.Interfaces;
using Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReactiveUI;

namespace AvaloniaUIClient;

public partial class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);
    
    private static IHost? _host;
    
    public static IHost? Host => _host ??= HostCreator.CreateHost(IocWorker.RegistredServies());
    
    public static IServiceProvider Services => Host!.Services;
    
    public override async void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            RxApp.DefaultExceptionHandler = Services.GetRequiredService<IObserver<Exception>>();

            desktop.MainWindow = Services.GetRequiredService<MainWindow>();
            
           await Host!.StartAsync();
        }
        
        base.OnFrameworkInitializationCompleted();

        StartupEvents();
    }
    
    private void StartupEvents()
    {
        Services.GetService<SettingsFileService>().Get();
        
        Services.GetService<UserFleService>().Get();

        Services.GetService<ITokenService>().Refresh();
        
        Services.GetService<BaseVmdNavigationService<ITitleVmd>>();
    }
}