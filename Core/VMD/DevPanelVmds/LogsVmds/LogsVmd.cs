using System.Collections.ObjectModel;
using System.Reactive;
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


namespace Core.VMD.DevPanelVmds;

public sealed class LogsVmd : BaseCollectionVmd<LogEvent>
{

    #region Logs Collection fields

    private readonly IStore<ObservableCollection<LogEvent>>? _logStore;
    public SelectedItems<LogEventLevel,LogLevelSelector> LogLevelsSelector { get; }

    #endregion
    
    public ObservableCollection<MenuParamCommandItem> LoggerTests { get; }

    public  LogsSettingsVmd LogsSettingsVmd { get; }
    
    public LogsVmd(IStore<ObservableCollection<LogEvent>> logStore, ILogger<LogsVmd> logger)
    {
        #region Fields|Properties initialize

        _logStore = logStore;
        
        LogsSettingsVmd = HostWorker.Services.GetRequiredService<LogsSettingsVmd>();

        LogLevelsSelector = new SelectedItems<LogEventLevel,LogLevelSelector>((LogEventLevel[]) Enum.GetValues(typeof(LogEventLevel)));
        
        #endregion
        
        #region Subscriptions

        logStore.CurrentValueChangedNotifier += () =>  DoSearch(SearchText);

        LogLevelsSelector
            .AllItems
            .ToObservableChangeSet()
            .AutoRefresh(x => x.IsAdd)
            .Subscribe(_=>DoSearch(SearchText));
        
        #endregion

        #region Commands

        LoggerTest = ReactiveCommand.Create<LogLevel>(level =>
        {
            logger.Log(level, $"Test {level}");
        });

        
        ClearCollection = ReactiveCommand.Create(() =>
        {
            logStore?.CurrentValue.Clear();
            DoSearch(SearchText);
        });
        
        #endregion
        
        #region Collections initialize
        
        LoggerTests = new ObservableCollection<MenuParamCommandItem>(MenuParamCommandItem.CreateCollectionWithEnumParameter(LoggerTest,Enum.GetValues<LogLevel>()));
        
        #endregion
    }

    #region Commands
    public ReactiveCommand<LogLevel,Unit> LoggerTest { get; }
    
    #endregion
    
    protected override void DoSearch(string? searchText)
    {
        Collection = _logStore?.CurrentValue
            .Where(x=> LogLevelsSelector.Filter!.Count != 0 ? 
                LogLevelsSelector.Filter.Items.ToList().Contains(x.Level) : true)
            .Where(x=> !string.IsNullOrEmpty(searchText) ? 
                x.RenderMessage().ToLower().Contains(searchText.ToLower(), StringComparison.InvariantCultureIgnoreCase) : true)!;
    }
    
}