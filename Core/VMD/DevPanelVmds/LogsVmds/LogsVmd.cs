using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using AppInfrastructure.Stores.DefaultStore;
using Core.Infrastructure.Hosting;
using Core.Infrastructure.Models;
using Core.Infrastructure.Models.ItemSelectors;
using Core.Infrastructure.VMD;
using DynamicData;
using DynamicData.Binding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Serilog.Events;

namespace Core.VMD.DevPanelVmds.LogsVmds;

public sealed class LogsVmd : BaseCollectionVmd<LogEvent>
{
    #region Properties
    
    [Reactive]
    public new ObservableCollectionExtended<LogEvent> Collection { get; set; } = new ();
    
    public SelectedItems<LogEventLevel,LogLevelSelector> LogLevelsSelector { get; }
    
    public ObservableCollection<MenuParamCommandItem> LoggerTests { get; }
    
    public  LogsSettingsVmd LogsSettingsVmd { get; }
    
    #endregion

    #region Fields
    
    private readonly IObservable<Func<LogEvent, bool>> _categoryFilter;

    private readonly IObservable<Func<LogEvent, bool>> _searchFilter;

    private IDisposable? _disposeCollectionSubscriptions;

    private readonly IStore<ObservableCollection<LogEvent>> _store;
    
    #endregion

    #region Consturctors

    public LogsVmd(IStore<ObservableCollection<LogEvent>> logStore, ILogger<LogsVmd> logger)
    {
        #region Commands

        LoggerTest = ReactiveCommand.Create<LogLevel>(level =>  logger.Log(level, $"Test {level}"));
        
        ClearCollection = ReactiveCommand.Create(() => logStore.CurrentValue.Clear());
        
        #endregion
        
        #region Collections initialize
        
        LoggerTests = new ObservableCollection<MenuParamCommandItem>(MenuParamCommandItem.CreateCollectionWithEnumParameter(LoggerTest,Enum.GetValues<LogLevel>()));
        
        #endregion
        
        #region Fields|Properties initialize

        _store = logStore;
        
        LogsSettingsVmd = HostWorker.Services.GetRequiredService<LogsSettingsVmd>();

        LogLevelsSelector = new SelectedItems<LogEventLevel,LogLevelSelector>((LogEventLevel[]) Enum.GetValues(typeof(LogEventLevel)));
        
        _categoryFilter =       
            LogLevelsSelector
            .Filter
            .Connect()
            .ToCollection()
            .Select(CategoryFilterBuilder);
        
        _searchFilter =
            this.WhenValueChanged(x => x.SearchText)
                .Throttle(TimeSpan.FromMilliseconds(250))
                .Select(SearchFilterBuilder);
        
        #endregion
        
        _store.CurrentValueDeletedNotifier += SetCollectionSubscriptions;
        
        SetCollectionSubscriptions();
        
    }
    #endregion
    
    #region Commands
    public ReactiveCommand<LogLevel,Unit> LoggerTest { get; }
    
    #endregion

    #region Methods

    private Func<LogEvent, bool> SearchFilterBuilder(string? searchText)
    {
        searchText = searchText?.Trim();
        
        if (string.IsNullOrEmpty(searchText)) return x => true;

        return x => x.RenderMessage().ToLower().Contains(searchText.ToLower(),
            StringComparison.InvariantCultureIgnoreCase);
    }
    
    
    private Func<LogEvent, bool> CategoryFilterBuilder(IEnumerable<LogEventLevel> elems)
    {
        var logEventLevels = elems.ToList();
        
        if (logEventLevels.Count == 0) return x => true;

        return x=> logEventLevels.Contains(x.Level);
    }

    private void SetCollectionSubscriptions()
    {
        _disposeCollectionSubscriptions?.Dispose();

        Collection?.Clear();
        
        _disposeCollectionSubscriptions = 
            _store.CurrentValue
            .ToObservableChangeSet()
            .Filter(_searchFilter)
            .Filter(_categoryFilter)
            .Bind(Collection)
            .DisposeMany()
            .Subscribe(_=>{});

        // Only for initizlie. IDK, but without this it doesnt working
        LogLevelsSelector.AllItems.First().IsAdd = !LogLevelsSelector.AllItems.First().IsAdd;
        LogLevelsSelector.AllItems.First().IsAdd = !LogLevelsSelector.AllItems.First().IsAdd;
    }

    #endregion
}