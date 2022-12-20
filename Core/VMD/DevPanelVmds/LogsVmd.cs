using System.Collections.ObjectModel;
using AppInfrastructure.Stores.DefaultStore;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Serilog.Events;

namespace Core.VMD.DevPanelVmds;

public sealed class LogsVmd : ReactiveObject
{
    [Reactive]
    public ObservableCollection<LogEvent> Logs { get; set; }
    
    public LogsVmd(IStore<ObservableCollection<LogEvent>> logs)
    {
        logs.CurrentValueDeletedNotifier += () => Logs = logs.CurrentValue;

        Logs = logs.CurrentValue;
    }
}