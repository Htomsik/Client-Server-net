﻿using System.Collections.ObjectModel;
using System.Reactive.Linq;
using AppInfrastructure.Stores.Repositories.Collection;
using Core.Infrastructure.Hosting;
using Core.VMD.DevPanelVmds;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Serilog.Events;

namespace Core.VMD;

public class MainVmd : ReactiveObject
{
    [Reactive]
    public LogEvent? LastLog { get; private set; }

    private DevPanelVmd? DevPanelVmd { get; }
    
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

        DevPanelVmd = (DevPanelVmd?)HostWorker.Services.GetService(typeof(DevPanelVmd));
    }
}