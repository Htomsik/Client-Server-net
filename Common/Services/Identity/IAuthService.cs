using Domain.identity;

namespace Services.Identity;

public interface IAuthService
{
    Task<Tokens?> Authorize(LoginUserDTO user);

    Task<Tokens?> Registration(LoginUserDTO user);
    
    Task<Tokens?> RefreshTokens(Tokens tokens);
}