using System;
using System.Windows;
using Core.Infrastructure.Hosting;
using Core.VMD;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVVMBase.IOC;
using MVVMBase.Views;
using ReactiveUI;

namespace MVVMBase
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IHost? _host;
        public static IHost? Host => _host ??= HostCreator.CreateHost(IocWorker.RegistredServies());
        public static IServiceProvider Services => Host.Services;
        
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            MainWindow = Services.GetRequiredService<MainWindow>();

            MainWindow.Show();

            await Host.StartAsync();
        }
        
    }
}