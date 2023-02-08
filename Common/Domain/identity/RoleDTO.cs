using Interfaces.Entities;

namespace Domain.identity;

public class RoleDTO : INamedEntity
{
    public int Id { get; set; }
    
    public string Name { get; set; }
}