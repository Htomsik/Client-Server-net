using Interfaces.Entities;

namespace Core.Infrastructure.Services.AccountService;

public interface IAccountService<in TAuthUser, in TRegUser>
    where TAuthUser : IAuthUser
    where TRegUser : IRegUser
{
    public Task<bool> Authorization(TAuthUser authUser,CancellationToken cancel = default);

    public Task<bool> Registration(TRegUser authUser,CancellationToken cancel = default);

    public Task<bool> Deactivate(CancellationToken cancel = default);
}