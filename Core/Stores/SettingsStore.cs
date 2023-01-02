using Core.Infrastructure.Models.SettingsModels;
using Core.Infrastructure.Stores;

namespace Core.Stores;

public class SettingsStore : BaseReactiveStore<Settings>
{
    public SettingsStore():base(new Settings()){}
}
