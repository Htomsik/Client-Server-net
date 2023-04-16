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
        services.AddHttpClient<IAuthService<AuthUser,RegUser, Tokens>, AuthorizationHttpService<AuthUser,RegUser, Tokens>>(client =>
        {
            client.BaseAddress = new Uri($"{configuration["API"]}/api/Account/");
        });

        return services;
    }
}
        