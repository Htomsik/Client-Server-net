namespace API.Infrastructure.DI;

internal static partial class DiRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services)
        => services
            .AddSingleton<Startup>();
}