﻿using System.Collections.ObjectModel;
using AppInfrastructure.Stores.DefaultStore;
using AppInfrastructure.Stores.Repositories.Collection;
using Core.Infrastructure.Logging;
using Core.Infrastructure.Stores;
using Core.VMD.Base;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Events;

namespace Core.Infrastructure.IOC;

public partial class IocRegistration
{
    public static IServiceCollection StoresRegistration(this IServiceCollection services) =>
        services
            .InfrStoresRegs()
            .VmdsStoreRegs();
    
    private static IServiceCollection VmdsStoreRegs(this IServiceCollection services) =>
        services.AddSingleton<IStore<ITitleVmd>, TitleVmdStore>();

    private static IServiceCollection InfrStoresRegs(this IServiceCollection services) =>
        services
            .AddSingleton<ICollectionRepository<ObservableCollection<LogEvent>, LogEvent>, LogStore>()
            .AddSingleton <IStore<ObservableCollection<LogEvent>>>(s=> s.GetRequiredService<ICollectionRepository<ObservableCollection<LogEvent>, LogEvent>>());

}