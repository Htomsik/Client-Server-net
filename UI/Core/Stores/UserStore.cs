using Core.Infrastructure.Models.Entities;
using Core.Infrastructure.Stores;

namespace Core.Stores;

public sealed class UserStore : BaseTimerReactiveStore<User>
{
    public UserStore():base(new User()){}
    protected override int InitialTimerSeconds { get; } = 1;
}