using Microsoft.Extensions.DependencyInjection;

namespace Core.IOC;

public static partial class IocRegistrator
{
    public static IServiceCollection ServiceRegistration(this IServiceCollection services) =>
        services;
}