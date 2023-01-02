﻿using Core.Infrastructure.Logging;
using Core.Infrastructure.Services.FileService;
using Core.Infrastructure.Services.NavigationService;
using Core.Infrastructure.Services.ParseService;
using Core.Infrastructure.VMD;
using Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.IOC;

public static partial class IocRegistration
{
    public static IServiceCollection ServiceRegistration(this IServiceCollection services) =>
        services
            .InfrServicesRegs()
            .NavServicesRegs()
            .AdditionalServiceRegs();

    private static IServiceCollection NavServicesRegs(this IServiceCollection services) =>
        services.AddSingleton<BaseVmdNavigationService<ITitleVmd>, TitleVmdsNavigationService>();

    private static IServiceCollection AdditionalServiceRegs(this IServiceCollection services) =>
        services.AddSingleton<SettingsFileService>();
    
    private static IServiceCollection InfrServicesRegs(this IServiceCollection services) =>
        services
            .AddTransient<IParseService,ParseService>()
            .AddSingleton<IObserver<Exception>,GlobalExceptionHandler>()
            .AddSingleton<InfoToLogSink>();
}