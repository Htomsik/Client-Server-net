using System.Collections.ObjectModel;
using AppInfrastructure.Stores.DefaultStore;
using Core.VMD.Base;
using ReactiveUI;
using Serilog.Events;

namespace Core.VMD.DevPanelVmds;

public sealed class LogsVmd : BaseCollectionVmd<LogEvent>
{
    
    private readonly IStore<ObservableCollection<LogEvent>>? _logStore;

    public LogsVmd(IStore<ObservableCollection<LogEvent>> logStore)
    {
        _logStore = logStore;
        
        #region Subscriptions

        logStore.CurrentValueChangedNotifier += () =>  DoSearch(SearchText);
       
        this.WhenAnyValue(x => x.SearchText).Subscribe(DoSearch);

        #endregion

        #region Commands

        ClearSearchText = ReactiveCommand.Create(() => SearchText = string.Empty );

        #endregion

    }

    #region Commands

    private IReactiveCommand ClearSearchText { get; }

    #endregion

    protected override void DoSearch(string? searchText)
    {
        if(!string.IsNullOrEmpty(searchText))
            Collection = _logStore.CurrentValue
                .Where(x => x.RenderMessage().ToLower()
                    .Contains(searchText.ToLower(),StringComparison.InvariantCultureIgnoreCase));
        else if (_logStore?.CurrentValue != Collection)
            Collection = _logStore.CurrentValue;
       
    }
    
}