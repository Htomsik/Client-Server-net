using Core.Infrastructure.IOC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReactiveUI;

namespace Core.Infrastructure.Hosting;

public static class HostCreator
{
    public static IHost? CreateHost(IServiceCollection? services = null)
    {
        IocRegistrator.SetAdditionalServices(services);
        
        RxApp.DefaultExceptionHandler = HostWorker.Services.GetRequiredService<IObserver<Exception>>();
        
        return HostWorker.Host;
    }
    
    
    
    
}