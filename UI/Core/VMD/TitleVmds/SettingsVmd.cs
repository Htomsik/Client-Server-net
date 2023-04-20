using AppInfrastructure.Stores.DefaultStore;
using Core.Infrastructure.Models.Other;
using Core.Infrastructure.Models.Settings;
using Core.Infrastructure.Services.Other;
using Core.Infrastructure.VMD;
using Core.VMD.MenuVmd;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Core.VMD.TitleVmds;

public sealed class SettingsVmd : BaseTitleVmd
{
    public override string Title  => "Settings";
    
    [Reactive] public Settings Settings { get; private set; }
    
    public SettingsVmd(
        IStore<Settings> settings)
    {
        Settings = settings.CurrentValue;

        settings.CurrentValueChangedNotifier += () => Settings = settings.CurrentValue;
    }
}