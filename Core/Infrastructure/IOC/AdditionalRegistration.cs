using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Core.Infrastructure.IOC;

public partial class IocRegistration
{
    private static IServiceCollection? _additionalServices;
    
    public static void AdditionalRegistration(this IServiceCollection? service) => service?.Add(_additionalServices);
   
    public static void SetAdditionalServices(IServiceCollection? services) => _additionalServices = services;
}