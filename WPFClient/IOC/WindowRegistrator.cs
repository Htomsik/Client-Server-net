using Core.VMD;
using Microsoft.Extensions.DependencyInjection;
using MVVMBase.Views;

namespace MVVMBase.IOC;

internal static partial class IocRegistrator
{
    public static IServiceCollection WindowRegistrator(this IServiceCollection service) =>
        service
            .AddSingleton(s => new MainWindow
            {
                DataContext = s.GetRequiredService<MainVmd>()
            });
}