using System.Collections.ObjectModel;
using System.Reactive.Linq;
using AppInfrastructure.Stores.DefaultStore;
using AppInfrastructure.Stores.Repositories.Collection;
using Core.Infrastructure.Hosting;
using Core.Infrastructure.Services.NavigationService;
using Core.VMD.Base;
using Core.VMD.DevPanelVmds;
using Core.VMD.TitleVmds;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Serilog.Events;

namespace Core.VMD;

public class MainVmd : BaseVmd
{

    #region Properties

    [Reactive]
    public LogEvent? LastLog { get;  set; }

    public IBaseVmd? DevPanelVmd { get; }
    
    public IBaseVmd? MainMenuVmd { get; }
    
    [Reactive]
    public ITitleVmd? TitleVmd { get; private set; }


    #endregion
   
    public MainVmd(ICollectionRepository<ObservableCollection<LogEvent>,LogEvent> logStore,IStore<ITitleVmd> titleVmdStore)
    {
        
        #region Subscriptions

        logStore.CurrentValueChangedNotifier += () => LastLog = logStore.CurrentValue.Last();

        titleVmdStore.CurrentValueChangedNotifier += () => TitleVmd = titleVmdStore.CurrentValue;
        
        #endregion
        
        #region Additional subs
        
        this
            .WhenAnyValue(x => x.LastLog)
            .Throttle(TimeSpan.FromSeconds(6))
            .Subscribe(_ => LastLog = null);
        
        #endregion

        #region Initializing

        DevPanelVmd = (IBaseVmd?)HostWorker.Services.GetService(typeof(DevVmd));

        MainMenuVmd = (IBaseVmd?)HostWorker.Services.GetService(typeof(MainMenuVmd));

        HostWorker.Services.GetService<BaseVmdNavigationService<ITitleVmd>>()!.Navigate(typeof(HomeVmd));

        #endregion

    }
}