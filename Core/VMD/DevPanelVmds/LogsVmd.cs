using System.Collections.ObjectModel;
using AppInfrastructure.Stores.DefaultStore;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Serilog.Events;

namespace Core.VMD.DevPanelVmds;

public sealed class LogsVmd : ReactiveObject
{
    [Reactive]
    public IEnumerable<LogEvent> Logs { get; set; }
    
    [Reactive]
    public string SearchText { get; set; }

    private IStore<ObservableCollection<LogEvent>> _logStoreStore;

    public LogsVmd(IStore<ObservableCollection<LogEvent>> logStore)
    {
        _logStoreStore = logStore;
        
        #region Subscriptions

        logStore.CurrentValueChangedNotifier += () =>
        {
            Logs = logStore.CurrentValue;
            DoSearch(SearchText);
        };

        this.WhenAnyValue(x => x.SearchText).Subscribe(DoSearch);

        #endregion

        #region Commands

        ClearSearchText = ReactiveCommand.Create(() => SearchText = string.Empty );

        #endregion

    }

    #region Commands

    private IReactiveCommand ClearSearchText { get; }

    #endregion
    
    private void DoSearch(string? searchText)
    {
        if(!string.IsNullOrEmpty(searchText))
            Logs = Logs.Where(x => x.RenderMessage().Contains(searchText));
        else
            Logs = _logStoreStore?.CurrentValue;
       
    }
}