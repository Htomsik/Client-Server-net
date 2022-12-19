using AvaloniaUIClient.Views;
using Core.VMD;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaUIClient.Infrastucture.IOC;

internal static partial class IocRegistrator
{
    public static IServiceCollection WindowRegistrator(this IServiceCollection service) =>
        service
            .AddSingleton(s => new MainWindow
            {
                DataContext = s.GetRequiredService<MainVmd>()
            });
}