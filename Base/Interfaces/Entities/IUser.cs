using Interfaces.Other;

namespace Interfaces.Entities;

public interface IUser : INamedEntity
{
    public ITokens Tokens { get; set; }
}