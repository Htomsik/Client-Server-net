using AppInfrastructure.Stores.DefaultStore;

namespace Core.Infrastructure.Stores.Interfaces;

public interface ITimerStore<T> : IStore<T>
{
    public event Action<long> TimerChangeNotifier;
    
}