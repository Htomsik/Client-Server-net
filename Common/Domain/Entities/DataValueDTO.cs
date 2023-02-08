using Interfaces.Entities;

namespace Domain.Entities;

public class DataValueDTO : IEntity
{
    public int Id { get; set; }
    
    public string Value { get; set; }
    
    public DateTimeOffset Time { get; set; }
    
    public bool IsFault { get; set; }
}