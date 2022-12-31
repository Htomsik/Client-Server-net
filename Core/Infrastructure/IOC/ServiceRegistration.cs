using Core.Infrastructure.Logging;
using Core.Infrastructure.Services;
using Core.VMD.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.IOC;

public static partial class IocRegistration
{
    public static IServiceCollection ServiceRegistration(this IServiceCollection services) =>
        services
            .InfrServicesRegs()
            .NavServicesRegs();

    public static IServiceCollection NavServicesRegs(this IServiceCollection services) =>
        services.AddSingleton<BaseVmdNavigationService<ITitleVmd>, TitleVmdsNavigationService>();
    
    private static IServiceCollection InfrServicesRegs(this IServiceCollection services) =>
        services
            .AddSingleton<IObserver<Exception>,GlobalExceptionHandler>()
            .AddSingleton<InfoToLogSink>();
}