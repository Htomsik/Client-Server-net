using Interfaces.Other;

namespace Interfaces.Entities;

public interface IUser : IAuthUser
{
    public ITokens Tokens { get; set; }
    
}

public interface IAuthUser : INamedEntity
{
    public string Password { get; set; }
}

public interface IRegUser : IAuthUser
{
    public string Email { get; set; }
}