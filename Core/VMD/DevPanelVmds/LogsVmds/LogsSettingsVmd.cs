using AppInfrastructure.Stores.DefaultStore;
using Core.Infrastructure.Models.ItemSelectors;
using Core.Infrastructure.Models.Settings;
using Core.Infrastructure.VMD;
using ReactiveUI.Fody.Helpers;
using Serilog.Events;

namespace Core.VMD.DevPanelVmds.LogsVmds;

public sealed class LogsSettingsVmd : BaseVmd
{
    #region Properties
    [Reactive]
    public  Settings Settings { get; private set; }
    
    public SelectedItems<LogEventLevel,LogLevelSelector> LogLevelsSelector { get; private set; }

    #endregion

    #region Constructors
    public LogsSettingsVmd(IStore<Settings> settings)
    {
        #region Subscriptions
        
        settings.CurrentValueChangedNotifier += () => OnSettingsChanged(settings.CurrentValue);
        
        #endregion

        #region Initializiang
        
        OnSettingsChanged(settings.CurrentValue);
        
        #endregion
    }
    #endregion

    #region Methods
    private void OnSettingsChanged(Settings settings)
    {
        Settings = settings;
            
        LogLevelsSelector = new SelectedItems<LogEventLevel,LogLevelSelector>(new []
        {
            LogEventLevel.Information,
            LogEventLevel.Warning,
            LogEventLevel.Error,
            LogEventLevel.Fatal
        }, settings.ShowedLogLevels);
    }
    private Func<LogEventLevel, bool> CategoryFilterBuilder(IEnumerable<LogEventLevel> elems)
    {
        var logEventLevels = elems.ToList();
        
        if (logEventLevels.Count == 0) return _ => true;

        return x=> logEventLevels.Contains(x);
    }
    #endregion
}