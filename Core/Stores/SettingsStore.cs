using Core.Infrastructure.Models.Settings;
using Core.Infrastructure.Stores;

namespace Core.Stores;

internal sealed class SettingsStore : BaseTimerReactiveStore<Settings>
{
    public SettingsStore():base(new Settings()){}
    protected override int InitialTimerSeconds { get; } = 10;
}
