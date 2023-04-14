using System.Net.Http.Headers;
using Domain.identity;
using HTTPClients.Repositories;
using HTTPClients.Services;
using Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models.Data;
using Models.Identity;
using Services.Identity;

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

    private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
    {
        services.AddHttpClient<HttpRepository<DataSource>, HttpRepository<DataSource>>(client =>
        {
            client.BaseAddress = new Uri($"{host.Configuration["API"]}/api/DataSource/");
        });

        services.AddHttpClient<IAuthService<LoginUserDTO, Tokens>, AuthorizationHttpService<LoginUserDTO, Tokens>>(client =>
        {
            client.BaseAddress = new Uri($"{host.Configuration["API"]}/api/Account/");
        });
    }

    #endregion

    static async Task Main()
    {
        #region Initialize

        using var host = Hosts;

        await host.StartAsync();
        
        #endregion

        #region Params

        var dsWebRepository = host.Services.GetService<HttpRepository<DataSource>>();

        var accountWebRepository = host.Services.GetService<IAuthService<LoginUserDTO,Tokens>>();
        
        var dataSource = new DataSource
        {
            Id=0,
            Description = "",
            Name = ""
        };

        var user = new UserDTO()
        {
            Name = "",
            Password = "",
            Email = "testEmail@gmail.com",
            Tokens = new Tokens
            {
                Token = "",
                RefreshToken = ""
            }
        };
        
        Boolean isWorking = true;
        
        #endregion
        
        #region Methods

        async Task Stats()
        {
            Console.WriteLine($"> Exist: {await dsWebRepository.Exist(dataSource)}");
        
            Console.WriteLine($"> Exist_id: {await dsWebRepository.Exist(dataSource.Id)}");
            
            Console.WriteLine($"> Count: [{await dsWebRepository.Count()}]");

            dataSource = await dsWebRepository.Get(dataSource.Id);
            
            Console.WriteLine($">>> Get \n > Id:{dataSource.Id} \n > Name:{dataSource.Name} \n > Description:{dataSource.Description}");
            
        }

        void ShowLocalInfo()
        {
            Console.Clear();
            Console.WriteLine("------>Local info<-----");
            Console.WriteLine($">>> ACC \n > Name:{user.Name} \n > Password:{user.Password}");
            Console.WriteLine($">>> Tokens \n > AuthToken:{user.Tokens.Token} \n > RefreshToken:{user.Tokens.RefreshToken}");
            Console.WriteLine($">>> DS \n > Id:{dataSource.Id} \n > Name:{dataSource.Name} \n > Description:{dataSource.Description}");
            Console.WriteLine("------->--------<------");
        }

        void InitDS()
        {
            
            Console.WriteLine(">>>  Init DS Value");
            
            Console.Write("> Description:");
            
            dataSource.Description = Console.ReadLine();

            ShowLocalInfo();
            
            Console.WriteLine(">>>  Init Value");
            
            Console.Write("> Name:");
            
            dataSource.Name = Console.ReadLine();
        }
        
        void InitACC()
        {
            ShowLocalInfo();
            
            Console.WriteLine(">>>  Init ACC Value");
            
            Console.Write("> Name:");
            
            user.Name = Console.ReadLine();

            ShowLocalInfo();
            
            Console.WriteLine(">>>  Init ACC Value");
            
            Console.Write("> Password:");
            
            user.Password = Console.ReadLine();
        }

        async Task RegistrationAcc()
        {
            user.Tokens = await accountWebRepository.Registration(user).ConfigureAwait(false);

            if (user.Tokens.Token != null)
                dsWebRepository.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(user.Tokens.Token);
        }
        
        #endregion
        
        Console.WriteLine(">>>  Host creation completed");
        
        Console.WriteLine(">>>  Wait enter");
        
        while (isWorking)
        {
            ShowLocalInfo();
            
            Console.WriteLine(">>>  Select operation:"); 
            
            Console.WriteLine(">>> ACC \n > initACC \n > RegACC \n > AuthACC \n > RefreshTokenACC");
            
            Console.WriteLine(">>> DS \n > AddDS \n > DeleteDS \n > UpdateDS \n > initDS \n > StatsDS \n > Close");

            var operation = Console.ReadLine()?.ToLower();
            
            switch (operation)
            {
                #region Ds

                case "addds":
                    Console.WriteLine($">>> Add: {await dsWebRepository!.Add(dataSource!)}");
                    break;
                
                case "updateds":
                    Console.WriteLine($">>> Update: {await dsWebRepository.Update(dataSource)}");
                    break;
                
                case "deleteds":
                    Console.WriteLine($">>> Delete: {await dsWebRepository.Delete(dataSource)}");
                    break;
                
                case "statsds":
                    Console.WriteLine($">>> Stats:");
                    await Stats();
                    break;
                
                case "initds":
                    InitDS();
                    break;

                #endregion

                #region Acc

                case "initacc":
                    InitACC();
                    break;
                
                case  "regacc":
                    await RegistrationAcc();
                    break;
                
                #endregion
                
                case "close":
                    Console.WriteLine($">>> Close App");
                    isWorking = false;
                    break;
                
                default: 
                    Console.WriteLine(">>> Invalid operation");
                    break;
            }
            
        }
    }
}