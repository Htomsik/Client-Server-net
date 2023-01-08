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
using Serilog.Events;

namespace Core.VMD.DevPanelVmds.LogsVmds;

public sealed class LogsVmd : BaseCollectionVmd<LogEvent>
{

    #region Properties

    public SelectedItems<LogEventLevel,LogLevelSelector> LogLevelsSelector { get; }
    
    public new ObservableCollectionExtended<LogEvent> Collection { get; set; } =
        new ();
    
    public ObservableCollection<MenuParamCommandItem> LoggerTests { get; }
    
    public  LogsSettingsVmd LogsSettingsVmd { get; }

    #endregion

    #region Consturctors

    public LogsVmd(IStore<ObservableCollection<LogEvent>> logStore, ILogger<LogsVmd> logger)
    {
        #region Commands

        LoggerTest = ReactiveCommand.Create<LogLevel>(level =>  logger.Log(level, $"Test {level}"));
        
        ClearCollection = ReactiveCommand.Create(() => logStore?.CurrentValue.Clear());
        
        #endregion
        
        #region Collections initialize
        
        LoggerTests = new ObservableCollection<MenuParamCommandItem>(MenuParamCommandItem.CreateCollectionWithEnumParameter(LoggerTest,Enum.GetValues<LogLevel>()));
        
        Collection = new ObservableCollectionExtended<LogEvent>(logStore.CurrentValue);
        
        #endregion
        
        #region Fields|Properties initialize
        
        LogsSettingsVmd = HostWorker.Services.GetRequiredService<LogsSettingsVmd>();

        LogLevelsSelector = new SelectedItems<LogEventLevel,LogLevelSelector>((LogEventLevel[]) Enum.GetValues(typeof(LogEventLevel)));
        
        #endregion
        
        #region Subscriptions
        
        var searchFilter = 
            this.WhenValueChanged(x => x.SearchText)
                .Throttle(TimeSpan.FromMilliseconds(250))
                .Select(SearchFilter);
        
        var categoryFilter =
            LogLevelsSelector
                .Filter
                .Connect()
                .ToCollection()
                .Select(CategoryFilter);
        
        logStore
            .CurrentValue
            .ToObservableChangeSet()
            .Filter(searchFilter)
            .Filter(categoryFilter)
            .Bind(Collection)
            .Subscribe(_ => { });
        #endregion
    }
    #endregion
    
    #region Commands
    public ReactiveCommand<LogLevel,Unit> LoggerTest { get; }
    
    #endregion

    #region Methods

    private static Func<LogEvent, bool> SearchFilter(string? searchText)
    {
        if (string.IsNullOrEmpty(searchText)) return x => true;

        return x => x.RenderMessage().ToLower().Contains(searchText.ToLower(),
            StringComparison.InvariantCultureIgnoreCase);
    }
    
    
    private static Func<LogEvent, bool> CategoryFilter(IEnumerable<LogEventLevel> elems)
    {
        var logEventLevels = elems.ToList();
        
        if (logEventLevels.Count == 0) return x => true;

        return x=> logEventLevels.Contains(x.Level);
    }

    #endregion
    
}