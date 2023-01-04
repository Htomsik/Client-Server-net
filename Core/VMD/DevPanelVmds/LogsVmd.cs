﻿using System.Collections.ObjectModel;
using System.Reactive;
using AppInfrastructure.Stores.DefaultStore;
using AppInfrastructure.Stores.Repositories.Collection;
using Core.Infrastructure.Models;
using Core.Infrastructure.Models.SettingsModels;
using Core.Infrastructure.VMD;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Serilog.Events;


namespace Core.VMD.DevPanelVmds;

public sealed class LogsVmd : BaseCollectionVmd<LogEvent>
{

    #region Logs Collection fields

    private readonly IStore<ObservableCollection<LogEvent>>? _logStore;

    private readonly BaseLazyCollectionRepository<List<LogEventLevel>,LogEventLevel> _selectedLogLevels  = new ();
    public ObservableCollection<LogLevelSelected>? AllLogLevels { get; }

    #endregion
    
    public ObservableCollection<MenuParamCommandItem> LoggerTests { get; }

    private readonly ILogger _logger;

    [Reactive]
    public  Settings Settings { get; set; }


    public LogsVmd(IStore<ObservableCollection<LogEvent>> logStore, ILogger<LogsVmd> logger,IStore<Settings> settings)
    {
        #region Fields|Properties initialize

        _logStore = logStore;
        
        _logger = logger;
        
        Settings = settings.CurrentValue;

        #endregion
        
        #region Subscriptions

        logStore.CurrentValueChangedNotifier += () =>  DoSearch(SearchText);
        
        _selectedLogLevels.CurrentValueChangedNotifier += () => DoSearch(SearchText);
        
        settings.CurrentValueChangedNotifier += () => Settings = settings.CurrentValue;
        
        #endregion

        #region Commands

        LoggerTest = ReactiveCommand.Create<LogLevel>(level =>
        {
            _logger.Log(level, $"Test {level}");
        });

        ClearFilters = ReactiveCommand.Create(()=> 
        {
            foreach (var item in AllLogLevels)
            {
                item.IsAddedToFilter = false;
            }
        });

        ClearCollection = ReactiveCommand.Create(() =>
        {
            logStore?.CurrentValue.Clear();
            DoSearch(SearchText);
        });
        
        #endregion
        
        #region Collections initialize

        AllLogLevels = new ObservableCollection<LogLevelSelected>(LogLevelSelected.CreateAllLevelsCollection(_selectedLogLevels));

        LoggerTests = new ObservableCollection<MenuParamCommandItem>(MenuParamCommandItem.CreateCollectionWithEnumParameter(LoggerTest,Enum.GetValues<LogLevel>()));
        
        #endregion
    }

    #region Commands

    public ReactiveCommand<LogLevel,Unit> LoggerTest { get; }
    
    public IReactiveCommand ClearFilters { get; }

    #endregion
    
    protected override void DoSearch(string? searchText)
    {
        Collection = _logStore?.CurrentValue
            .Where(x=> _selectedLogLevels?.CurrentValue?.Count != 0 ? 
                _selectedLogLevels.CurrentValue.Contains(x.Level) : true)
            .Where(x=> !string.IsNullOrEmpty(searchText) ? 
                x.RenderMessage().ToLower().Contains(searchText.ToLower(), StringComparison.InvariantCultureIgnoreCase) : true);
    }

   
    
}