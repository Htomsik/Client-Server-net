using System.Collections.ObjectModel;
using AppInfrastructure.Stores.DefaultStore;
using AppInfrastructure.Stores.Repositories.Collection;
using Core.Infrastructure.Models;
using Core.VMD.Base;
using ReactiveUI;
using Serilog.Events;


namespace Core.VMD.DevPanelVmds;

public sealed class LogsVmd : BaseCollectionVmd<LogEvent>
{
    
    private readonly IStore<ObservableCollection<LogEvent>>? _logStore;

    private readonly BaseLazyCollectionRepository<List<LogEventLevel>,LogEventLevel> _selectedLogLevels  = new ();
    public ObservableCollection<LogLevelSelected>? AllLogLevels { get; }

    public LogsVmd(IStore<ObservableCollection<LogEvent>> logStore)
    {
        _logStore = logStore;

        #region Initializing

        AllLogLevels = new ObservableCollection<LogLevelSelected>(LogLevelSelected.CreateAllLevelsCollection(_selectedLogLevels));
        
        #endregion
        
        #region Subscriptions

        logStore.CurrentValueChangedNotifier += () =>  DoSearch(SearchText);
       
        this.WhenAnyValue(x => x.SearchText).Subscribe(DoSearch);

        _selectedLogLevels.CurrentValueChangedNotifier += () => DoSearch(SearchText);

        #endregion

    }
    
    protected override void DoSearch(string? searchText)
    {
        Collection = _logStore!.CurrentValue
            .Where(x=> _selectedLogLevels.CurrentValue.Count != 0 ? 
                _selectedLogLevels.CurrentValue.Contains(x.Level) : true)
            .Where(x=> !string.IsNullOrEmpty(searchText) ? 
                x.RenderMessage().ToLower().Contains(searchText.ToLower(), StringComparison.InvariantCultureIgnoreCase) : true);
    }

   
    
}