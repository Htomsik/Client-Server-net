using AppInfrastructure.Stores.DefaultStore;

namespace Core.Infrastructure.Stores;

public interface ITimerStore<T> : IStore<T>
{
    public event Action<long> TimerChangeNotifier;
    
}