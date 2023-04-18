using Core.VMD;
using Core.VMD.DevPanelVmds;
using Core.VMD.DevPanelVmds.LogsVmds;
using Core.VMD.TitleVmds;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.IOC;

public partial class IocRegistration
{
    public static IServiceCollection VmdRegistration(this IServiceCollection services) =>
        services
            .InfrVmdsRegs()
            .TitleVmdRegs();

    private static IServiceCollection TitleVmdRegs(this IServiceCollection services) =>
        services
            .AddTransient<HomeVmd>()
            .AddTransient<SettingsVmd>();

    private static IServiceCollection InfrVmdsRegs(this IServiceCollection service) =>
        service
            .AddSingleton<MainVmd>()
            .AddSingleton<MainMenuVmd>()
            .AddSingleton<StatusLineVmd>()
            .AddSingleton<DevVmd>()
            .AddSingleton<LogsVmd>()
            .AddTransient<LogsSettingsVmd>()
            .AddSingleton<StoresVmd>()
            .AddSingleton<NotificationsVmd>()
            .AddTransient<AboutProgramVmd>();
}