using AvaloniaUIClient.Infrastructure.Services.Other;
using Core.Infrastructure.Services.Other;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaUIClient.Infrastructure.IOC;

internal static partial class IocRegistrator
{
    public static IServiceCollection ServiceRegistrator(this IServiceCollection services) =>
        services
            .AddTransient<IUiThreadOperation,UiThreadOperation>();
}