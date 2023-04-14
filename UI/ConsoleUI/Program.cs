using HTTPClients.Repositories;
using Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models.Data;

namespace ConsoleUI;

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
        services.AddHttpClient<IRepository<DataSource>,HttpRepository<DataSource>>(client =>
        {
            client.BaseAddress = new Uri($"{host.Configuration["API"]}/api/DataSource/");
        });
    #endregion

    static async Task Main()
    {
        #region Initialize

        using var host = Hosts;

        await host.StartAsync();
        
        #endregion

        #region Params

        var webRepository = host.Services.GetService<IRepository<DataSource>>()!;
        
        var editedDs = new DataSource
        {
            Id=1213,
            Description = "ConsoleUI",
            Name = "HttpTesterName"
        };

        Boolean isWorking = true;
        
        #endregion
        
        #region Methods

        async Task Stats()
        {
            Console.WriteLine($"> Exist: {await webRepository.Exist(editedDs)}");
        
            Console.WriteLine($"> Exist_id: {await webRepository.Exist(editedDs.Id)}");
            
            Console.WriteLine($"> Count: [{await webRepository.Count()}]");

            editedDs = await webRepository.Get(editedDs.Id);
            
            Console.WriteLine($">>> Get \n > Id:{editedDs.Id} \n > Name:{editedDs.Name} \n > Description:{editedDs.Description}");
            
        }
        #endregion
        
        Console.WriteLine(">>>     Host creation completed     <<<");
        
        while (isWorking)
        {
            Console.WriteLine(">>>  Wait enter.....");
        
            Console.ReadLine();
            
            Console.Clear();
            
            Console.WriteLine(">>>  Select operation: \n >Add \n >Delete \n >Update \n >Stats \n >Close");

            var operation = Console.ReadLine()?.ToLower();
            
            switch (operation)
            {
                case "add":
                    Console.WriteLine($">>> Add: {await webRepository!.Add(editedDs!)}"); 
                    break;
                
                case "update":
                    editedDs.Description = "TestDesc";
                    editedDs.Name = "TestDesc";
                    Console.WriteLine($">>> Update: {await webRepository.Update(editedDs)}");
                    break;
                
                case "delete":
                    Console.WriteLine($">>> Delete: {await webRepository.Delete(editedDs)}");
                    break;
                
                case "stats":
                    Console.WriteLine($">>> Stats:");
                    await Stats();
                    break;
                
                case "close":
                    Console.WriteLine($">>> Close App....");
                    isWorking = false;
                    break;
                
                default: 
                    Console.WriteLine(">>> Invalid operation");
                    break;
            }
            
        }
    }
}