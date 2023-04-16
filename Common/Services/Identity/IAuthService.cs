using Interfaces.Other;

namespace Services.Identity;

public interface IAuthService<in TAuthUser, TRegUser, TTokens>
    where  TTokens : ITokens
{
    Task<TTokens?> Authorize(TAuthUser user, CancellationToken cancel = default);

    Task<TTokens?> Registration(TRegUser user, CancellationToken cancel = default);
    
    Task<TTokens?> RefreshTokens(TTokens tokens, CancellationToken cancel = default);
}