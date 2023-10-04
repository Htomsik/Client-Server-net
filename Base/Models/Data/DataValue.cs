using Microsoft.EntityFrameworkCore;
using Models.Base;

namespace Models.Data;

[Index(nameof(Time))]
public class DataValue : Entity
{
    public DateTimeOffset Time { get; set; } = DateTimeOffset.Now;
    
    public string Value { get; set; }
    
    public DataSource Source { get; set; }
    
    public bool IsFault { get; set; }
    
}