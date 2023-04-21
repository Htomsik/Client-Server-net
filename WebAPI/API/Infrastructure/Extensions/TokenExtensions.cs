using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Interfaces.Other;

namespace API.Infrastructure.Extensions;

internal static class TokenExtensions
{
    public static string? UserName(this ITokens tokens)
    {
        var jwtSecurity = new JwtSecurityTokenHandler();

        var content = jwtSecurity.ReadJwtToken(tokens.Token);
        
        var userName = content.Claims.ToList().FirstOrDefault(elem => elem.Type == ClaimTypes.Name)?.Value;

        return userName;
    }
}