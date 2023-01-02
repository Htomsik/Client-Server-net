using AppInfrastructure.Stores.DefaultStore;
using Core.Infrastructure.Models.SettingsModels;
using Core.Infrastructure.VMD;
using ReactiveUI.Fody.Helpers;

namespace Core.VMD.TitleVmds;

public sealed class SettingsVmd : BaseTitleVmd
{
    public override string Title  => "Settings";
    
    [Reactive]
    public Settings Settings { get; private set; }

    public SettingsVmd(IStore<Settings> settings)
    {
        Settings = settings.CurrentValue;
        
    }
}