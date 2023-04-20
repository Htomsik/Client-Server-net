namespace Core.Infrastructure.Services.AccountService;

public interface ITokenService
{
    public Task<bool> Refresh(CancellationToken cancel = default);
}