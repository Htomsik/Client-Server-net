using API.Data;
using API.Infrastructure.Configuration;
using API.Infrastructure.IOC;
using API.MiddleWare;
using DAL.Context;
using Microsoft.AspNetCore.Identity;
using Models.Identity;

namespace API;

public record Startup(IConfiguration Configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddDataBase(Configuration)
            .AddServices();

        services
            .AddIdentity<User, Role>()
            .AddEntityFrameworkStores<DataDb>()
            .AddTokenProvider(Configuration["Security:JWT:Issuer"], typeof(DataProtectorTokenProvider<User>));
        
        services.AddEndpointsApiExplorer();
        
        services.AddControllers();
        
        services.ConfigureSwagger();
    }

    public void Configure(IApplicationBuilder app, 
        IWebHostEnvironment env, 
        IDbInitializer db, 
        ILogger<Startup> logger)
    {
        if (!db.Initialize().Result)
        {
            logger.LogCritical("Db initializing failed. Application startup terminated");
            Environment.FailFast("Application startup terminated. Db can't initialized");
        }
        
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();
        
        app.UseRouting();

        app.UseAuthorization();
        
        app.UseMiddleware<JwtMiddleware>();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}