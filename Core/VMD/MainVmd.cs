using System.Collections.ObjectModel;
using System.Reactive.Linq;
using AppInfrastructure.Stores.Repositories.Collection;
using Core.Infrastructure.Hosting;
using Core.VMD.Base;
using Core.VMD.DevPanelVmds;
using Core.VMD.TitleVmds;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Serilog.Events;

namespace Core.VMD;

public class MainVmd : BaseVmd
{
    [Reactive]
    public LogEvent? LastLog { get;  set; }

    public IBaseVmd? DevPanelVmd { get; }
    
    public IBaseVmd? MainMenuVmd { get; }
    
    [Reactive]
    public ITitleVmd? TitleVmd { get; private set; }
    
    public MainVmd(ICollectionRepository<ObservableCollection<LogEvent>,LogEvent> logStore)
    {
        
        #region Subscriptions

        logStore.CurrentValueChangedNotifier += () => LastLog = logStore.CurrentValue.Last();
        
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
        
        TitleVmd = (ITitleVmd?)HostWorker.Services.GetService(typeof(HomeVmd));
        
        #endregion
       
    }
}