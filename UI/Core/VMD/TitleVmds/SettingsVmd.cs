using AppInfrastructure.Stores.DefaultStore;
using Core.Infrastructure.Models.Settings;
using Core.Infrastructure.VMD;
using Core.Infrastructure.VMD.Interfaces;
using ReactiveUI.Fody.Helpers;

namespace Core.VMD.TitleVmds;

public sealed class SettingsVmd : BaseTitleVmd
{
    public override string Title  => "Settings";
    
    public ITitleVmd AccountVmd { get; }
    
    [Reactive] public Settings Settings { get; private set; }
    
    public SettingsVmd(
        AccountVmd accountVmd,
        IStore<Settings> settings)
    {
        Settings = settings.CurrentValue;

        AccountVmd = accountVmd;
        
        settings.CurrentValueChangedNotifier += () => Settings = settings.CurrentValue;
    }
}