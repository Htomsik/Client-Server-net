using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Core.Infrastructure.IOC;

public partial class IocRegistrator
{
    private static IServiceCollection? _additionalServices;
    
    public static void AdditionalRegistrator(this IServiceCollection? service) => service?.Add(_additionalServices);
   
    public static void SetAdditionalServices(IServiceCollection? services) => _additionalServices = services;
}