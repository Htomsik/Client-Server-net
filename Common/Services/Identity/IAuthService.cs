using Interfaces.Other;

namespace Services.Identity;

public interface IAuthService<in TUser, TTokens>
    where  TTokens : ITokens 
{
    Task<TTokens?> Authorize(TUser user, CancellationToken cancel = default);

    Task<TTokens?> Registration(TUser user, CancellationToken cancel = default);
    
    Task<TTokens?> RefreshTokens(TTokens tokens, CancellationToken cancel = default);
}