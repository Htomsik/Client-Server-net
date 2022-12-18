using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaUIClient.IOC;

internal static class IocWorker
{
    public static IServiceCollection RegistredServies() =>
        new ServiceCollection()
            .WindowRegistrator();

}