using API.Data;
using API.Infrastructure.IOC;

namespace API;

public record Startup(IConfiguration Configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddDataBase(Configuration)
            .AddServices();
        
        services.AddEndpointsApiExplorer();
        
        services.AddControllers();
        
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInitializer db)
    {
        db.Initialize();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseWebAssemblyDebugging();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();
        
        app.UseHttpsRedirection();
        
        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapFallbackToFile("index.html");
        });
    }
}