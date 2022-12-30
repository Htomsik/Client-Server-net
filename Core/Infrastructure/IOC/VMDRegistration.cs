﻿using Core.VMD;
using Core.VMD.DevPanelVmds;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.IOC;

public partial class IocRegistration
{
    public static IServiceCollection VmdRegistration(this IServiceCollection services) =>
        services
            .InfrVmdsRegs();
            

    private static IServiceCollection InfrVmdsRegs(this IServiceCollection service) =>
        service
            .AddSingleton<MainVmd>()
            .AddSingleton<MainMenuVmd>()
            .AddSingleton<DevVmd>()
            .AddSingleton<LogsVmd>();
}