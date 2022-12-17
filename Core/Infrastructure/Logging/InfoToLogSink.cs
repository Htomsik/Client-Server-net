using System.Collections.ObjectModel;
using AppInfrastructure.Stores.Repositories.Collection;
using Serilog.Core;
using Serilog.Events;

namespace Core.Infrastructure.Logging;

internal sealed class InfoToLogSink : ILogEventSink
{
    
    private readonly ICollectionRepository<ObservableCollection<LogEvent>,LogEvent>  _LogStore;

    public InfoToLogSink(ICollectionRepository<ObservableCollection<LogEvent>, LogEvent> logStore) =>
        _LogStore = logStore;
   
    public void Emit(LogEvent logEvent) => _LogStore.AddIntoEnumerable(logEvent);
    
}