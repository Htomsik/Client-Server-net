using Interfaces.Entities;

namespace Core.Infrastructure.Services.AccountService;

public interface IAccountService<TAutUser> where TAutUser : IAuthUser
{
    public Task<bool> Authorization(TAutUser authUser,CancellationToken cancel = default);

    public Task<bool> Registration(TAutUser authUser,CancellationToken cancel = default);
}