using Interfaces.Entities;

namespace Models.Base;

public abstract class Entity : IEntity
{
    public int Id { get; set; }
}