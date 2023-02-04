using System.Collections.ObjectModel;
using AppInfrastructure.Stores.Repositories.Collection;
using Serilog.Core;
using Serilog.Events;

namespace Core.Infrastructure.Logging;

internal sealed class InfoToLogSink : ILogEventSink
{
    private readonly ICollectionRepository<ObservableCollection<LogEvent>,LogEvent>  _logStore;

    public InfoToLogSink(ICollectionRepository<ObservableCollection<LogEvent>, LogEvent> logStore) =>
        _logStore = logStore;
   
    public void Emit(LogEvent logEvent) => _logStore.AddIntoEnumerable(logEvent);
}