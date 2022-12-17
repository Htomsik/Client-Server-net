using System.Collections.ObjectModel;
using AppInfrastructure.Stores.Repositories.Collection;
using Microsoft.Extensions.Configuration;
using ReactiveUI;
using Serilog.Events;

namespace Core.Infrastructure.Logging;

internal sealed class LogStore : BaseLazyCollectionRepository<ObservableCollection<LogEvent>,LogEvent>
{
    private int _maxLogs;
    
    protected override bool addIntoEnumerable(LogEvent value)
    {
        CurrentValue?.Add(value);
        return true;
    }

    public LogStore(IConfiguration configuration)
    {
        #region Subscriptions

        configuration
            .WhenAnyValue(x=>x["Settings:MaxLogs"])
            .Subscribe(_ =>
            {
                _maxLogs  = Convert.ToInt32(configuration["Settings:MaxLogs"]);
            });   
        
        configuration["Settings:MaxLogs"] = "100";
        
        #endregion

    }
    
}