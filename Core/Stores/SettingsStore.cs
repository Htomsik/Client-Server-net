using Core.Infrastructure.Models.SettingsModels;
using Core.Infrastructure.Stores;
using ReactiveUI;

namespace Core.Stores;

public class SettingsStore : BaseTimerReactiveStore<Settings>
{
    public SettingsStore():base(new Settings(),10){}
}
