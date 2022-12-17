using Core.IOC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace Core.Infrastucture.Hosting;

public static class HostCreator
{
    public static IHostBuilder CreateHostBuilder(string[] Args) =>
        Host
            .CreateDefaultBuilder(Args)
            .ConfigureServices(ConfigureServices)
            .UseSerilog((context, services, configuration) =>
            {
                configuration
                    .WriteTo.File(@"logs\Log-.txt", rollingInterval: RollingInterval.Day,restrictedToMinimumLevel: LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
            });


    public static void ConfigureServices(HostBuilderContext host, IServiceCollection services) =>
        services
            .StoresRegistrator()
            .ServiceRegistration()
            .VMDRegistrator();


}