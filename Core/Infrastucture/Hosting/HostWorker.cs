using Core.IOC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace Core.Infrastucture.Hosting;

internal class HostWorker
{
    private static IHost? _host;
    public static IHost? Host
        => _host ??= CreateHostBuilder(Environment.GetCommandLineArgs()).Build();
    
    public static IServiceProvider Services => Host.Services;
    
    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Microsoft.Extensions.Hosting.Host
            .CreateDefaultBuilder(args)
            .ConfigureServices(ConfigureServices)
            .UseSerilog((context, services, configuration) =>
            {
                configuration
                    .WriteTo.File(@"logs\Log-.txt", rollingInterval: RollingInterval.Day,restrictedToMinimumLevel: LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
            });
    
    private static void ConfigureServices(HostBuilderContext host, IServiceCollection services) =>
        services
            .StoresRegistrator()
            .ServiceRegistration()
            .VMDRegistrator()
            .AdditionalRegistrator();
}