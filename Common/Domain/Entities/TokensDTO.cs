using Interfaces.Other;

namespace Domain.Entities;

public class TokensDTO : ITokens
{
    public string Token { get; set; }
    
    public string RefreshToken { get; set; }
}