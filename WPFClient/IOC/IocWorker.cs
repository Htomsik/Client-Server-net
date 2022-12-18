using Microsoft.Extensions.DependencyInjection;

namespace MVVMBase.IOC;

internal static class IocWorker
{
    public static IServiceCollection RegistredServies() =>
        new ServiceCollection()
            .WindowRegistrator();

}