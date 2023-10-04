using Interfaces.Other;

namespace Models.Identity;

public class Tokens : ITokens
{
    public string Token { get; set; }
    
    public string? RefreshToken { get; set; }
}