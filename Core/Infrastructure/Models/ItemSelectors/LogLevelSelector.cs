using Serilog.Events;

namespace Core.Infrastructure.Models.ItemSelectors;

public sealed class LogLevelSelector : ItemSelector<LogEventLevel>
{
    #region Constructors
    
    public LogLevelSelector() {}
    
    #endregion
    
}