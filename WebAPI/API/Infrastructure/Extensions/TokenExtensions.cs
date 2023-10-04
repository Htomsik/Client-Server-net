using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Interfaces.Other;
using Microsoft.IdentityModel.Tokens;

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
    
    public static bool Validate(this ITokens tokens, IConfiguration jwtConfiguration)
    {
        var jwtSecurity = new JwtSecurityTokenHandler();
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration["Key"]));

        try
        {
            jwtSecurity.ValidateToken(tokens.Token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtConfiguration["Issuer"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateLifetime = true,
                ValidateAudience = false
            }, out SecurityToken validToken);
            
        }
        catch
        {
            return false;
        }

        return true;
    }
}