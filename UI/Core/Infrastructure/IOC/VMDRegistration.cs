using Core.VMD;
using Core.VMD.DevPanelVmds;
using Core.VMD.DevPanelVmds.LogsVmds;
using Core.VMD.TitleVmds;
using Core.VMD.TitleVmds.Account;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.IOC;

public partial class IocRegistration
{
    public static IServiceCollection VmdRegistration(this IServiceCollection services) =>
        services
            .InfrVmdsRegs()
            .TitleVmdRegs()
            .DialogVmdRegs();

    private static IServiceCollection TitleVmdRegs(this IServiceCollection services) =>
        services
            .AddTransient<HomeVmd>()
            .AddTransient<SettingsVmd>()
            .AddTransient<AccountVmd>();

    private static IServiceCollection DialogVmdRegs(this IServiceCollection services) =>
        services
            .AddTransient<AuthorizationVmd>()
            .AddTransient<RegistrationVmd>();

    private static IServiceCollection InfrVmdsRegs(this IServiceCollection service) =>
        service
            .AddSingleton<MainVmd>()
            .AddSingleton<MainMenuVmd>()
            .AddSingleton<StatusLineVmd>()
            .AddSingleton<DevVmd>()
            .AddSingleton<LogsVmd>()
            .AddTransient<LogsSettingsVmd>()
            .AddSingleton<StoresVmd>()
            .AddSingleton<AccountDevVmd>()
            .AddTransient<AboutProgramVmd>();
}