using API.Services;
using DAL.Repositories;
using Domain.identity;
using Interfaces.Repositories;
using Models.Identity;
using Services.Identity;

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
            .AddAutoMapper(typeof(Startup))
            .AddScoped(typeof(IRepository<>),typeof(DbRepository<>))
            .AddScoped(typeof(INamedRepository<>),typeof(DbNameRepository<>))
            .AddScoped<IAuthService<LoginUserDTO,RegistratonUserDTO, Tokens>, AuthService>();
}