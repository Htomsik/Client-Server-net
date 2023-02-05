using DAL.Repositories;
using Interfaces.Repositories;

namespace API.Infrastructure.IOC;

internal static partial class IoCRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services)
        => services
            .AddInfrastructureServices()
            .AddBusinessServices();
    
    private static IServiceCollection AddBusinessServices(this IServiceCollection services) =>
        services;
    
    private static IServiceCollection AddInfrastructureServices(this IServiceCollection services) =>
        services
            .AddScoped(typeof(IRepository<>),typeof(DbRepository<>))
            .AddScoped(typeof(INamedRepository<>),typeof(DbNameRepository<>));
}