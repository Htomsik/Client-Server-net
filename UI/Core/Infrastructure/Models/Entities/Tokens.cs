using Interfaces.Other;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Core.Infrastructure.Models.Entities;

public sealed class Tokens : ReactiveObject, ITokens
{
    [Reactive] public string Token { get; set; }
    
    [Reactive] public string RefreshToken { get; set; }

    public Tokens(){}
    
    public Tokens(ITokens tokens)
    {
        Token = tokens.Token;

        RefreshToken = tokens.RefreshToken;
    }
}