using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.IOC;

public partial class IocRegistrator
{
    public static IServiceCollection StoresRegistrator(this IServiceCollection services) =>
        services;
}