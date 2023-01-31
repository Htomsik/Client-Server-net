namespace API.Infrastructure.IOC;

internal static partial class IoCRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services)
        => services
            .AddSingleton<Startup>();
}