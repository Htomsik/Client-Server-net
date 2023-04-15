namespace Core.Infrastructure.Services.AccountService;

public interface IAccountService
{
    public Task<bool> Authorization(CancellationToken cancel = default);

    public Task<bool> Registration(CancellationToken cancel = default);
}