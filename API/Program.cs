using API.Infrastructure.DI;

namespace API;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args);
        
        await host.RunAsync();
        
        host.Services.GetService<Startup>().Initialize();
    }

    #region Methods

    private static WebApplication CreateHostBuilder(string[] args)
    {
        var host = WebApplication.CreateBuilder(args);
        
        host.ConfigureDefaultServices();
        
        host.ConfigureServices();

       var buildHost = host.Build();
        
       buildHost.ConfigureAfterBuild();
        
       return buildHost;
    }
    
    private static void ConfigureDefaultServices(this WebApplicationBuilder builder) =>
        builder.Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddControllers();
    
    private static void ConfigureServices(this WebApplicationBuilder builder) =>
        builder.Services
            .AddDataBase(builder.Configuration);

    private static void ConfigureAfterBuild(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
    }
    
    #endregion
    
}