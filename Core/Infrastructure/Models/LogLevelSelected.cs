using AppInfrastructure.Stores.Repositories.Collection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Serilog.Events;

namespace Core.Infrastructure.Models;

public sealed class LogLevelSelected : ReactiveObject
{
    #region Properties

    public LogEventLevel logLevel { get; init; }

    [Reactive]
    public bool IsAddedToFilter { get; set; }

    #endregion

    #region Constructor

    public LogLevelSelected(LogEventLevel level, ICollection<LogEventLevel> loglevels)
    {
        logLevel = level;

        this
            .WhenAnyValue(x => x.IsAddedToFilter)
            .Subscribe(_ =>
            {
                if (IsAddedToFilter)
                    loglevels.Add(logLevel);
                else
                    loglevels.Remove(logLevel);
            });
    }
    
    public LogLevelSelected(LogEventLevel level, ICollectionRepository<List<LogEventLevel>,LogEventLevel> loglevels)
    {
        logLevel = level;

        this
            .WhenAnyValue(x => x.IsAddedToFilter)
            .Subscribe(_ =>
            {
                if (IsAddedToFilter)
                    loglevels.AddIntoEnumerable(logLevel);
                else
                    loglevels.RemoveFromEnumerable(logLevel);
            });
    }


    #endregion

    #region Methods

    public static IEnumerable<LogLevelSelected> CreateAllLevelsCollection(ICollection<LogEventLevel> logLevelSelecteds)
    {
        foreach (LogEventLevel loglevel in (LogEventLevel[]) Enum.GetValues(typeof(LogEventLevel)))
        {
            yield return new LogLevelSelected(loglevel,logLevelSelecteds);
        }
    }
    
    public static IEnumerable<LogLevelSelected> CreateAllLevelsCollection(ICollectionRepository<List<LogEventLevel>,LogEventLevel> logLevelSelecteds)
    {
        foreach (LogEventLevel loglevel in (LogEventLevel[]) Enum.GetValues(typeof(LogEventLevel)))
        {
            yield return new LogLevelSelected(loglevel,logLevelSelecteds);
        }
    }

    #endregion
}