using Microsoft.Extensions.DependencyInjection;

namespace Core.IOC;

public partial class IocRegistrator
{
    public static IServiceCollection VMDRegistrator(this IServiceCollection services) =>
        services;
}