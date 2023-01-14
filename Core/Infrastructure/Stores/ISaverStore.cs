namespace Core.Infrastructure.Stores;

public interface ISaverStore<T, out TNotifyValue> : ITimerStore<T>
{
    new event Action<TNotifyValue>? CurrentValueChangedNotifier;
    
    void SaveNow();
    
    void SaveNowWithoutFile();
    
}