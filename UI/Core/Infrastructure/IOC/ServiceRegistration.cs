﻿using Core.Infrastructure.Extensions;
using Core.Infrastructure.Logging;
using Core.Infrastructure.Models.Entities;
using Core.Infrastructure.Services.AccountService;
using Core.Infrastructure.Services.DialogService;
using Core.Infrastructure.Services.EncryptService;
using Core.Infrastructure.Services.NavigationService;
using Core.Infrastructure.Services.Other;
using Core.Infrastructure.Services.ParseService;
using Core.Infrastructure.VMD.Interfaces;
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
        services
            .AddSingleton<SettingsFileService>()
            .AddSingleton<UserFleService>();
    
    private static IServiceCollection InfrServicesRegs(this IServiceCollection services) =>
        services
            .AddTransient<IHttpTokenService, HttpTokenService>()
            .AddTransient<IEncryptService, Base64EncryptService>()
            .AddTransient<IDecryptService>(s=>s.GetRequiredService<IEncryptService>())
            .AddSingleton<IDialogService, DialogService>()
            .AddSingleton<IVmdDialogService>(s=>s.GetRequiredService<IDialogService>())
            .AddSingleton<IViewDialogService>(s=>s.GetRequiredService<IVmdDialogService>())
            .AddTransient<IAccountService<AuthUser, RegUser>, AccountService>()
            .AddTransient<ITokenService, TokenService>()
            .AddSingleton<SettingsService>()
            .AddTransient<ProjectInfo>()
            .AddTransient<IParseService,ParseService>()
            .AddSingleton<IObserver<Exception>,GlobalExceptionHandler>()
            .AddSingleton<InfoToLogSink>();
}