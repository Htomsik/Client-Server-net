using Models.Base;

namespace Models.Data;

public class DataValue : Entity
{
    public DateTimeOffset Time { get; set; } = DateTimeOffset.Now;
    
    public string Value { get; set; }
    
    public DataSource Source { get; set; }
    
    public bool isFault { get; set; }
    
}