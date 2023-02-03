using HTTPClients.Repositories;
using Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models.Data;

namespace HttpTester;

public class Program
{
    #region Host

    private static IHost? _host;

    public static IHost Hosts => _host ??= CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

    #endregion

    #region Host creation method

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host
            .CreateDefaultBuilder(args)
            .ConfigureServices(ConfigureServices);

    private static void ConfigureServices(HostBuilderContext host, IServiceCollection services) =>
        services.AddHttpClient<HttpRepository<DataSource>>(client =>
        {
            client.BaseAddress = new Uri($"{host.Configuration["API"]}/api/DataSource/");
        });
    
    #endregion

    static async Task Main()
    {
        using var host = Hosts;

        await host.StartAsync();
        
        Console.WriteLine("Host creation completed");
    }
}