using System.Collections.ObjectModel;
using System.Reactive.Linq;
using AppInfrastructure.Stores.Repositories.Collection;
using Core.Infrastructure.Models.Settings;
using Core.Infrastructure.Stores.Interfaces;
using Core.Infrastructure.VMD;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Serilog.Events;

namespace Core.VMD;

public class StatusLineVmd : BaseVmd
{
    #region Properties

    [Reactive]
    public LogEvent? LastLog { get;  set; }

    [Reactive]
    public int SaveTimer { get; set; } = 0;

    #endregion
    
    #region Constructors

    public StatusLineVmd(
        ICollectionRepository<ObservableCollection<LogEvent>,LogEvent> logStore,
        ISaverStore<Settings,Boolean> settingsStore)
    {
        #region Subscriptions
        
        logStore.CurrentValueChangedNotifier += () =>
        {
            if (logStore?.CurrentValue?.Count() != 0 &&
                (bool)settingsStore?.CurrentValue?.ShowedLogLevels.Contains(logStore.CurrentValue.Last().Level))
                LastLog = logStore?.CurrentValue?.Last();
        };
        
        settingsStore.TimerChangeNotifier += (timer) => { SaveTimer = (int)timer; };

        this
            .WhenAnyValue(x => x.LastLog)
            .Throttle(TimeSpan.FromSeconds(6))
            .Subscribe(_ => LastLog = null);

        this.WhenAnyValue(x => x.SaveTimer)
            .Throttle(TimeSpan.FromSeconds(2))
            .Where(x=>x != 0)
            .Subscribe(_ => SaveTimer = 0);

        #endregion
        
        #region Commands initializing

        SaveSettings = ReactiveCommand.Create(settingsStore.SaveNow);

        #endregion
    }

    #endregion

    #region Commands

    private IReactiveCommand SaveSettings { get; } 

    #endregion
}