using Microsoft.Extensions.DependencyInjection;

namespace Core.IOC;

public partial class IocRegistrator
{
    public static IServiceCollection StoresRegistrator(this IServiceCollection services) =>
        services;
}