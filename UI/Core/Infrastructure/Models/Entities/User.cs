using System.Xml.Serialization;
using Interfaces.Entities;
using Interfaces.Other;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Core.Infrastructure.Models.Entities;

public class User : AuthUser
{
    [Reactive]
    public ITokens? Tokens { get; set; }
}

public class AuthUser : ReactiveObject
{
    [Reactive] public int Id { get; set; }
    
    [Reactive] public string? Name { get; set; }
    
    [Reactive] public string? Email { get; set; }
    
    [XmlIgnore] public string? Password { get; set; }
}