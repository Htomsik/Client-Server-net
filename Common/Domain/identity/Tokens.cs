using Interfaces.Other;

namespace Domain.identity;

public class Tokens : ITokens
{
    public string Token { get; set; }
    
    public string? RefreshToken { get; set; }
}