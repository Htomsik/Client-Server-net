using System.Reactive.Linq;
using DynamicData.Binding;
using ReactiveUI;

namespace Core.Infrastructure.Stores;

public abstract class BaseTimerReactiveStore<TValue> : BaseReactiveStore<TValue> where TValue : IReactiveObject
{
    private readonly int _initialTimerSeconds = 5;

    private IDisposable timer;

    #region TimerChangeNotifier

    public event Action<long>? TimerChangeNotifier;
    
    protected void OnTimerChangeNotifier(long currentSec) => TimerChangeNotifier?.Invoke(currentSec);

    #endregion
    
    public override TValue? CurrentValue
    {
        get => (TValue?)_currentValue.Value;
        set
        {
            _currentValue = new Lazy<object?>(()=> value);
            
            CurrentValue?
                .WhenAnyPropertyChanged()
                .Do(_=>StartTimer())
                .Throttle(TimeSpan.FromSeconds(_initialTimerSeconds))
                .Subscribe(_ => OnCurrentValueChanged());
            
            OnCurrentValueChanged();
        }
      
    }
    
    #region Constructors

    public BaseTimerReactiveStore(TValue value, int timerSecondsSpan) : base(value) =>
        _initialTimerSeconds = timerSecondsSpan;
    
    public BaseTimerReactiveStore(int timerSecondsSpan) : base() =>
        _initialTimerSeconds = timerSecondsSpan;
    
    public BaseTimerReactiveStore(TValue value) : base(value) {}

    public BaseTimerReactiveStore() : base() {}
    
    #endregion

    #region Method

    private void StartTimer()
    {
        timer?.Dispose();
        
        timer = Observable
            .Timer(DateTimeOffset.Now, TimeSpan.FromSeconds(1))
            .Select(currentSeconds => _initialTimerSeconds - currentSeconds)
            .TakeWhile(currentSeconds => currentSeconds >= 0)
            .Subscribe(OnTimerChangeNotifier);
    }

    #endregion
    
}