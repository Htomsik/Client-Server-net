using System.Reactive.Linq;
using DynamicData.Binding;
using ReactiveUI;

namespace Core.Infrastructure.Stores;

public abstract class BaseTimerReactiveStore<TValue> : BaseReactiveStore<TValue> where TValue : IReactiveObject
{
    protected abstract int InitialTimerSeconds { get; }

    private IDisposable? _timer;
    
    #region TimerChangeNotifier

    public event Action<long>? TimerChangeNotifier;
    
    protected void OnTimerChangeNotifier(long currentSec) => TimerChangeNotifier?.Invoke(currentSec);

    #endregion
    
    #region Constructors
    
    public BaseTimerReactiveStore(TValue value) : base(value){}
    
    public BaseTimerReactiveStore() : base(){}
  
    
    #endregion

    #region Methods

    private void StartTimer()
    {
        _timer?.Dispose();
        
        _timer = Observable
            .Timer(DateTimeOffset.Now, TimeSpan.FromSeconds(1))
            .Select(currentSeconds => InitialTimerSeconds - currentSeconds)
            .TakeWhile(currentSeconds => currentSeconds >= 0)
            .Subscribe(OnTimerChangeNotifier);
    }
    
    protected override void SetValueRelays()
    {
        CurrentValue?
            .WhenAnyPropertyChanged()
            .Do(_=>StartTimer())
            .Throttle(TimeSpan.FromSeconds(InitialTimerSeconds))
            .Subscribe(_ => OnCurrentValueChanged());
    }
    
    #endregion
    
}