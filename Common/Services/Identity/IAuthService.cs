using Domain.identity;
using Models.Identity;

namespace Services.Identity;

public interface IAuthService
{
    Task<Tokens?> Authorize(LoginUserDTO user);

    Task<Tokens?> Registration(LoginUserDTO user);
    
    Task<Tokens?> RefreshTokens(Tokens tokens);
}