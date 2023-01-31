using DAL.Repositories;
using Interfaces.Repositories;

namespace API.Infrastructure.IOC;

internal static partial class IoCRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services)
        => services
            .AddTransient<Startup>()
            .AddScoped(typeof(IRepository<>),typeof(DbRepository<>))
            .AddScoped(typeof(INamedRepository<>),typeof(DbNameRepository<>));
}