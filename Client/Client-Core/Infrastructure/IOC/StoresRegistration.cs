using System.Collections.ObjectModel;
using AppInfrastructure.Stores.DefaultStore;
using AppInfrastructure.Stores.Repositories.Collection;
using Core.Infrastructure.Logging;
using Core.Infrastructure.Models.Entities;
using Core.Infrastructure.Models.Settings;
using Core.Infrastructure.Stores.Interfaces;
using Core.Infrastructure.VMD.Interfaces;
using Core.Stores;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Events;

namespace Core.Infrastructure.IOC;

public partial class IocRegistration
{
    public static IServiceCollection StoresRegistration(this IServiceCollection services) =>
        services
            .InfrStoresRegs()
            .VmdsStoreRegs()
            .AllStoresRegs();

    private static IServiceCollection VmdsStoreRegs(this IServiceCollection services) =>
        services
            .AddSingleton<IStore<ITitleVmd>, TitleVmdStore>()
            .AddSingleton<ISaverStore<User, bool>, UserStore>()
            .AddSingleton<ITimerStore<User>>(s => s.GetRequiredService<ISaverStore<User, bool>>())
            .AddSingleton<IStore<User>>(s => s.GetRequiredService<ISaverStore<User, bool>>())
            .AddSingleton<ISaverStore<Settings, Boolean>, SettingsStore>()
            .AddSingleton<ITimerStore<Settings>>(s => s.GetRequiredService<ISaverStore<Settings, Boolean>>())
            .AddSingleton<IStore<Settings>>(s => s.GetRequiredService<ISaverStore<Settings, Boolean>>());

    private static IServiceCollection InfrStoresRegs(this IServiceCollection services) =>
        services
            .AddSingleton<ICollectionRepository<ObservableCollection<LogEvent>, LogEvent>, LogStore>()
            .AddSingleton <IStore<ObservableCollection<LogEvent>>>(s=> s.GetRequiredService<ICollectionRepository<ObservableCollection<LogEvent>, LogEvent>>());

    private static IServiceCollection AllStoresRegs(this IServiceCollection services) =>
        services
            .AddSingleton<IStore>(s=>s.GetRequiredService<IStore<ITitleVmd>>())
            .AddSingleton<IStore>(s=>s.GetRequiredService<IStore<Settings>>())
            .AddSingleton<IStore>(s=>s.GetRequiredService<IStore<ObservableCollection<LogEvent>>>());
}