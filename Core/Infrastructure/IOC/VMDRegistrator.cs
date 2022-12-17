using Core.VMD;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.IOC;

public partial class IocRegistrator
{
    public static IServiceCollection VMDRegistrator(this IServiceCollection services) =>
        services
            .AddSingleton<MainVmd>();
}