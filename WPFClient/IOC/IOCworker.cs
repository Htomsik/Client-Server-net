using Microsoft.Extensions.DependencyInjection;

namespace MVVMBase.IOC;

internal static class IOCworker
{
    public static IServiceCollection RegistredServies() =>
        new ServiceCollection()
            .WindowRegistrator();

}