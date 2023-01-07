using AppInfrastructure.Stores.DefaultStore;
using Core.Infrastructure.Models.SettingsModels;
using Core.Infrastructure.VMD;
using ReactiveUI.Fody.Helpers;

namespace Core.VMD.DevPanelVmds;

public sealed class LogsSettingsVmd : BaseVmd
{
    #region Properties

    [Reactive]
    public  Settings Settings { get; private set; }

    #endregion
    
    
    public LogsSettingsVmd(IStore<Settings> settings)
    {
        #region Properties Initialize

        Settings = settings.CurrentValue;

        #endregion

        #region Subscriptions

        settings.CurrentValueChangedNotifier += () => Settings = settings.CurrentValue;

        #endregion
    }
}