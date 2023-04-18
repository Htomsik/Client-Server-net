using AppInfrastructure.Stores.DefaultStore;
using Core.Infrastructure.Models.Settings;
using Core.Infrastructure.VMD;
using Core.VMD.MenuVmd;
using ReactiveUI.Fody.Helpers;

namespace Core.VMD.TitleVmds;

public sealed class SettingsVmd : BaseTitleVmd
{
    public override string Title  => "Settings";
    
    [Reactive]
    public Settings Settings { get; private set; }

    public ThemeVmd ThemeVmd { get; }

    public SettingsVmd(
        IStore<Settings> settings, 
        ThemeVmd themeVmd)
    {
        Settings = settings.CurrentValue;

        ThemeVmd = themeVmd;
    }
}