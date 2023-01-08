using AppInfrastructure.Stores.DefaultStore;
using Core.Infrastructure.Models;
using Core.Infrastructure.Models.ItemSelectors;
using Core.Infrastructure.Models.SettingsModels;
using Core.Infrastructure.VMD;
using ReactiveUI.Fody.Helpers;
using Serilog.Events;

namespace Core.VMD.DevPanelVmds;

public sealed class LogsSettingsVmd : BaseVmd
{
    #region Properties

    [Reactive]
    public  Settings Settings { get; private set; }
    
    public SelectedItems<LogEventLevel,LogLevelSelector> LogLevelsSelector { get; }

    #endregion
    
    public LogsSettingsVmd(IStore<Settings> settings)
    {
        #region Properties Initialize

        Settings = settings.CurrentValue;
        
        LogLevelsSelector = new SelectedItems<LogEventLevel,LogLevelSelector>(new []
        {
            LogEventLevel.Information,
            LogEventLevel.Warning,
            LogEventLevel.Error,
            LogEventLevel.Fatal
        }, settings.CurrentValue.ShowedLogLevels);
        
        #endregion

        #region Subscriptions

        settings.CurrentValueChangedNotifier += () => Settings = settings.CurrentValue;

        #endregion
        
    }
}