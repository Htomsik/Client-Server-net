using Core.Infrastructure.IOC;
using Core.Infrastructure.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Core.Infrastructure.Hosting;

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
            .ConfigureAppConfiguration((host, cfg) => cfg
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", false, true)
            )
            .UseSerilog((context, services, configuration) =>
            {
                configuration
                    .WriteTo.File(@"logs\Log-.txt", rollingInterval: RollingInterval.Day)
                    .WriteTo.Sink(services.GetRequiredService<InfoToLogSink>());
            });
    
    private static void ConfigureServices(HostBuilderContext host, IServiceCollection services) =>
        services
            .StoresRegistration()
            .ServiceRegistration()
            .VmdRegistration()
            .AdditionalRegistration();
}