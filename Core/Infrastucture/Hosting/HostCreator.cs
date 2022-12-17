using Core.IOC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Core.Infrastucture.Hosting;

public static class HostCreator
{
    public static IHost? GetHost(IServiceCollection? services = null)
    {
        IocRegistrator.SetAdditionalServices(services);

        return HostWorker.Host;
    }
    
}