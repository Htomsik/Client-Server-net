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
    
    public ObservableCollection<MenuParamCommandItem> LoggerTests { get; }
    
    public  LogsSettingsVmd LogsSettingsVmd { get; }
    
    #endregion

    #region Fields
    
    private readonly IObservable<Func<LogEvent, bool>> _categoryFilter;
    
    #endregion

    #region Consturctors

    public LogsVmd(IStore<ObservableCollection<LogEvent>> logStore, ILogger<LogsVmd> logger) : base(logStore.CurrentValue)
    {
        #region Commands

        LoggerTest = ReactiveCommand.Create<LogLevel>(level =>  logger.Log(level, $"Test {level}"));
        
        ClearCollection = ReactiveCommand.Create(() => logStore.CurrentValue.Clear());
        
        #endregion
        
        #region Collections initialize
        
        LoggerTests = new ObservableCollection<MenuParamCommandItem>(MenuParamCommandItem.CreateCollectionWithEnumParameter(LoggerTest,Enum.GetValues<LogLevel>()));
        
        #endregion
        
        #region Fields|Properties initialize
        
        LogsSettingsVmd = HostWorker.Services.GetRequiredService<LogsSettingsVmd>();

        LogLevelsSelector = new SelectedItems<LogEventLevel,LogLevelSelector>((LogEventLevel[]) Enum.GetValues(typeof(LogEventLevel)));
        
        #endregion

        #region Subscriptions

        logStore.CurrentValueDeletedNotifier += () =>
        {
            SetSubscriptions(logStore.CurrentValue);
            this.RaisePropertyChanged(nameof(Items));
        };
        
        
        _categoryFilter =       
            LogLevelsSelector
                .Filter
                .Connect()
                .ToCollection()
                .Select(CategoryFilterBuilder);

        #endregion
        
        SetCollectionSubscriptions(logStore.CurrentValue);
    }
    #endregion
    
    #region Commands
    public ReactiveCommand<LogLevel,Unit> LoggerTest { get; }
    
    #endregion

    #region Methods

    protected override Func<LogEvent, bool> SearchFilterBuilder(string? searchText)
    {
        searchText = searchText?.Trim();
        
        if (string.IsNullOrEmpty(searchText)) return x => true;

        return x => x.RenderMessage().Contains(searchText,
            StringComparison.InvariantCultureIgnoreCase);
    }
    
    private Func<LogEvent, bool> CategoryFilterBuilder(IEnumerable<LogEventLevel> elems)
    {
        var logEventLevels = elems.ToList();
        
        if (logEventLevels.Count == 0) return x => true;

        return x=> logEventLevels.Contains(x.Level);
    }
    
    protected override void SetCollectionSubscriptions(ObservableCollection<LogEvent> inputData)
    {
        if(!IsInitialize)
            return;
        
        DisposeSubscriptions = 
            inputData
                .ToObservableChangeSet()
                .Filter(SearchFilter)
                .Filter(_categoryFilter)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out ItemsSetter)
                .DisposeMany()
                .Subscribe(_=>{});

        // Only for initizlie. IDK, but without this it doesnt working
        LogLevelsSelector.AllItems.First().IsAdd = !LogLevelsSelector.AllItems.First().IsAdd;
        LogLevelsSelector.AllItems.First().IsAdd = !LogLevelsSelector.AllItems.First().IsAdd;
    }

    #endregion
}