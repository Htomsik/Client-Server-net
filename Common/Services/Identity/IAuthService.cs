using Interfaces.Other;

namespace Services.Identity;

public interface IAuthService<in TAuthUser, in TRegUser, TTokens>
    where  TTokens : ITokens
{
    Task<TTokens?> Authorize(TAuthUser user, CancellationToken cancel = default);

    Task<TTokens?> Registration(TRegUser user, CancellationToken cancel = default);

    Task<bool> Deactivate(TTokens tokens, CancellationToken cancel = default);
        
    Task<TTokens?> RefreshTokens(TTokens tokens, CancellationToken cancel = default);
}