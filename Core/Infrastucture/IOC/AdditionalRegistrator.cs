using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Core.IOC;

public partial class IocRegistrator
{
    private static IServiceCollection? additionalServices;

    public static IServiceCollection AdditionalRegistrator(this IServiceCollection service) => service.Add(additionalServices);

    public static void SetAdditionalServices(IServiceCollection? services) => additionalServices = services;
}