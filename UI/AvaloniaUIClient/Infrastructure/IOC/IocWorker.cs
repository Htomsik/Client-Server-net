using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaUIClient.Infrastructure.IOC;

internal static class IocWorker
{
    public static IServiceCollection RegistredServies() =>
        new ServiceCollection()
            .ServiceRegistrator()
            .WindowRegistrator();
}