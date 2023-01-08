using System.Collections.ObjectModel;
using AppInfrastructure.Stores.DefaultStore;
using AppInfrastructure.Stores.Repositories.Collection;
using Core.Infrastructure.Models.SettingsModels;
using Serilog.Events;

namespace Core.Infrastructure.Logging;

internal sealed class LogStore : BaseLazyCollectionRepository<ObservableCollection<LogEvent>,LogEvent>
{
    private int _maxLogs;
    
    protected override bool addIntoEnumerable(LogEvent value)
    {
        if(CurrentValue!.Count >= _maxLogs)
            RemoveLogs(1);
        
        CurrentValue?.Add(value);
        
        return true;
    }

    public LogStore(IStore<Settings> settings)
    {
        _maxLogs = settings.CurrentValue.DevLogsCount;
        
        #region Subscriptions

        settings.CurrentValueChangedNotifier += () =>
        {
            _maxLogs = settings.CurrentValue.DevLogsCount;

            if (CurrentValue!.Count >= _maxLogs)
                RemoveLogs(CurrentValue.Count - _maxLogs);
        };

        #endregion

    }

    void RemoveLogs(int toRemove)
    {
        if (CurrentValue?.Count == 0) return;
        
        var logsList = CurrentValue!.ToList();
        
        logsList.RemoveRange(0,toRemove);
        
        CurrentValue = new ObservableCollection<LogEvent>(logsList);
        
        OnCurrentValueDeleted();
    }
}