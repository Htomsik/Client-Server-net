using Core.Infrastructure.Models.Entities;
using HTTPClients.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Identity;

namespace Core.Infrastructure.IOC;

public partial class IocRegistration
{
    public static IServiceCollection HttpClientsRegistration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient<IAuthService<AuthUser, RegUser, User, Tokens>, AuthorizationHttpService<AuthUser,RegUser,User, Tokens>>(client =>
        {
            client.BaseAddress = new Uri($"{configuration["API"]}/api/Account/");
        });

        services.AddSingleton<AuthorizationHttpService<AuthUser, RegUser, User, Tokens>>(
            s => (AuthorizationHttpService<AuthUser, RegUser, User, Tokens>)s.GetRequiredService<IAuthService<AuthUser, RegUser, User, Tokens>>());
        
        return services;
    }
}
        