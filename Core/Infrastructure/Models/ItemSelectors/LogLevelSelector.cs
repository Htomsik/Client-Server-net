using AppInfrastructure.Stores.Repositories.Collection;
using Serilog.Events;

namespace Core.Infrastructure.Models.ItemSelectors;

public sealed class LogLevelSelector : ItemSelector<LogEventLevel>
{
    #region Constructors

    public LogLevelSelector(LogEventLevel item, ICollection<LogEventLevel> items) : base(item, items)
    {
    }

    public LogLevelSelector(LogEventLevel item, ICollectionRepository<List<LogEventLevel>, LogEventLevel> items) : base(item, items)
    {
    }

    #endregion
    
    #region Methods

    public static IEnumerable<LogLevelSelector> CreateAll(ICollection<LogEventLevel> items,LogEventLevel[]? levels = null)
    {
        levels ??= (LogEventLevel[]) Enum.GetValues(typeof(LogEventLevel));
        
        foreach (var level in levels)
        {
            yield return new LogLevelSelector(level, items);
        }
    }

    public static IEnumerable<LogLevelSelector> CreateAll(ICollectionRepository<List<LogEventLevel>, LogEventLevel> items,LogEventLevel[]? levels = null)
    {
        levels ??= (LogEventLevel[]) Enum.GetValues(typeof(LogEventLevel));
        
        foreach (var item in (LogEventLevel[]) Enum.GetValues(typeof(LogEventLevel)))
        {
            yield return new LogLevelSelector(item, items);
        }
    }
    
    #endregion
}