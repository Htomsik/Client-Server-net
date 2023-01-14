namespace Core.Infrastructure.Stores.Interfaces;

public interface ISaverStore<T, out TNotifyValue> : ITimerStore<T>
{
    new event Action<TNotifyValue>? CurrentValueChangedNotifier;
    
    void SaveNow();
    
    void SaveNowWithoutFile();
    
}