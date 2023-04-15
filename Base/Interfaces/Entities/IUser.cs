using Interfaces.Other;

namespace Interfaces.Entities;

public interface IUser : IAuthUser
{
    public ITokens Tokens { get; set; }
}

public interface IAuthUser : INamedEntity
{
    public string Email { get; set; }
    
    public string Password { get; set; }
}